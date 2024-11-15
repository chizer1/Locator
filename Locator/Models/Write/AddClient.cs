using System.ComponentModel.DataAnnotations;
using Locator.Domain;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class AddClient(string clientName, string clientCode, ClientStatus clientStatus)
{
    [Required]
    public string ClientName { get; set; } = clientName;

    [Required]
    public string ClientCode { get; set; } = clientCode;

    [Required]
    public ClientStatus ClientStatus { get; set; } = clientStatus;
}
