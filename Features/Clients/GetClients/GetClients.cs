using FluentValidation;
using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Clients.GetClients;

internal class GetClientsQuery(string keyword, int pageNumber, int pageSize)
{
    public string Keyword => keyword;
    public int PageNumber => pageNumber;
    public int PageSize => pageSize;
}

internal sealed class GetClientsQueryValidator : AbstractValidator<GetClientsQuery>
{
    public GetClientsQueryValidator()
    {
        RuleFor(x => x.PageNumber).NotEmpty().WithMessage("Page Number is required");
        RuleFor(x => x.PageSize).NotEmpty().WithMessage("Page Size is required");
    }
}

internal class GetClients(IClientRepository clientRepository)
{
    public async Task<PagedList<Client>> Handle(GetClientsQuery query)
    {
        await new GetClientsQueryValidator().ValidateAndThrowAsync(query);

        return await clientRepository.GetClients(query.Keyword, query.PageNumber, query.PageSize);
    }
}
