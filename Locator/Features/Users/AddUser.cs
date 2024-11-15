using System.Data.SqlClient;
using System.Text;
using Dapper;
using FluentValidation;
using Locator.Common;
using Locator.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Locator.Features.Users;

internal sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.UserStatus).NotEmpty().WithMessage("UserStatus is required.");
    }
}

public class AddUserCommand(
    string firstName,
    string lastName,
    string emailAddress,
    string password,
    UserStatus userStatus
)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string EmailAddress { get; } = emailAddress;
    public string Password { get; } = password;
    public UserStatus UserStatus { get; } = userStatus;
}

public class AddUser(SqlConnection locatorDb, string auth0Domain, Auth0 auth0)
{
    public async Task<int> Handle(AddUserCommand command)
    {
        await new AddUserCommandValidator().ValidateAndThrowAsync(command);

        var existingUser = await locatorDb.QueryFirstOrDefaultAsync<int?>(
            "select UserId from dbo.[User] where EmailAddress = @EmailAddress",
            new { command.EmailAddress }
        );

        if (existingUser != null)
            throw new ValidationException($"{command.EmailAddress} already exists");

        var accessToken = await auth0.GetAccessToken();

        dynamic userMetadata = new
        {
            email = command.EmailAddress,
            given_name = command.FirstName,
            family_name = command.LastName,
            name = $"{command.FirstName} {command.LastName}",
            password = command.Password,
            verify_email = false,
            connection = "Username-Password-Authentication",
        };
        string jsonContent = JsonConvert.SerializeObject(userMetadata);

        var requestUri = $"https://{auth0Domain}/api/v2/users";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        string auth0Id;
        if (response.IsSuccessStatusCode)
            auth0Id = JObject.Parse(responseString)["user_id"]?.ToString();
        else
            throw new Exception($"Auth0 Exception. Failed to create user: {responseString}");

        return await locatorDb.QuerySingleAsync<int>(
            @"
            insert into dbo.[User]
            (
                FirstName,
                LastName,
                EmailAddress,
                UserStatusID,
                Auth0ID
            )
            values
            (
                @FirstName,
                @LastName,
                @EmailAddress,
                @UserStatusID,
                @Auth0ID
            )

            select scope_identity()",
            new
            {
                command.FirstName,
                command.LastName,
                command.EmailAddress,
                UserStatusID = (int)command.UserStatus,
                auth0Id,
            }
        );
    }
}
