using FluentValidation;
using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Databases.GetDatabases;

internal class GetDatabasesQuery(string keyword, int pageNumber, int pageSize)
{
    public string Keyword => keyword;
    public int PageNumber => pageNumber;
    public int PageSize => pageSize;
}

internal sealed class GetDatabasesQueryValidator : AbstractValidator<GetDatabasesQuery>
{
    public GetDatabasesQueryValidator()
    {
        RuleFor(x => x.PageNumber).NotEmpty().WithMessage("Page Number is required");
        RuleFor(x => x.PageSize).NotEmpty().WithMessage("Page Size is required");
    }
}

internal class GetDatabases(IDatabaseRepository databaseRepository)
{
    public async Task<PagedList<Database>> Handle(GetDatabasesQuery query)
    {
        await new GetDatabasesQueryValidator().ValidateAndThrowAsync(query);

        return await databaseRepository.GetDatabases(
            query.Keyword,
            query.PageNumber,
            query.PageSize
        );
    }
}
