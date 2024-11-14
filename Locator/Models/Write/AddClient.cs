using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class AddClient(string clientName, string clientCode, ClientStatus clientStatus)
{
    [Required]
    public string ClientName { get; } = clientName;

    [Required]
    public string ClientCode { get; } = clientCode;

    [Required]
    public ClientStatus ClientStatus { get; } = clientStatus;
}
