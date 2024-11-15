using System.ComponentModel.DataAnnotations;
using Locator.Domain;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class UpdateClient(
    int clientId,
    string clientName,
    string clientCode,
    ClientStatus clientStatus
)
{
    [Required]
    public int ClientId { get; } = clientId;

    [Required]
    public string ClientName { get; } = clientName;

    [Required]
    public string ClientCode { get; } = clientCode;

    [Required]
    public ClientStatus ClientStatus { get; } = clientStatus;
}
