using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class AddUser(
    List<Role> roles,
    string firstName,
    string lastName,
    string emailAddress,
    string password,
    UserStatus userStatus
)
{
    [Required]
    public string FirstName { get; } = firstName;

    [Required]
    public string LastName { get; } = lastName;

    [Required]
    public string EmailAddress { get; } = emailAddress;

    [Required]
    public string Password { get; } = password;

    [Required]
    public List<Role> Roles { get; } = roles;

    [Required]
    public UserStatus UserStatus { get; } = userStatus;
}
