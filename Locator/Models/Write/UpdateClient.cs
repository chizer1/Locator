using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public class UpdateClient
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    public string ClientName { get; set; }

    [Required]
    public string ClientCode { get; set; }

    [Required]
    public ClientStatus ClientStatus { get; set; }
}