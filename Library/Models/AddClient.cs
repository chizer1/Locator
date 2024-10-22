using System.ComponentModel.DataAnnotations;

namespace Locator.Models;

public class AddClient
{
    [Required(ErrorMessage = "Client Name is required.")]
    [StringLength(50, ErrorMessage = "Client Name max length is 50 characters.")]
    public string ClientName { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Database Server is required.")]
    public int DatabaseServerId { get; set; }
}
