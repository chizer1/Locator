using System.ComponentModel.DataAnnotations;

namespace Locator.Models;

public class UpdateClient
{
    [Required(ErrorMessage = "Client Name is required.")]
    [StringLength(50, ErrorMessage = "Client Name max length is 50 characters.")]
    public string ClientName { get; set; }

    [Required(ErrorMessage = "Client Code is required.")]
    [StringLength(50, ErrorMessage = "Client Code max length is 20 characters.")]
    public string ClientCode { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Client Statis is required.")]
    public int ClientStatusId { get; set; }
}
