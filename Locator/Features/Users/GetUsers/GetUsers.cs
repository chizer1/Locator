using FluentValidation;
using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Users.GetUsers;

internal class GetUsersQuery(string keyword, int pageNumber, int pageSize)
{
    public string Keyword => keyword;
    public int PageNumber => pageNumber;
    public int PageSize => pageSize;
}

internal sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber).NotEmpty().WithMessage("Page Number is required");
        RuleFor(x => x.PageSize).NotEmpty().WithMessage("Page Size is required");
    }
}

internal class GetUsers(IUserRepository userRepository)
{
    public async Task<PagedList<User>> Handle(GetUsersQuery query)
    {
        await new GetUsersQueryValidator().ValidateAndThrowAsync(query);

        return await userRepository.GetUsers(query.Keyword, query.PageNumber, query.PageSize);
    }
}
