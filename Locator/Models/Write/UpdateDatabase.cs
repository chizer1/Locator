using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public class UpdateDatabase
{
    [Required]
    public int DatabaseId { get; set; }

    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string DatabaseUserName { get; set; }

    [Required]
    public int DatabaseServerId { get; set; }

    [Required]
    public int DatabaseTypeId { get; set; }

    [Required]
    public DatabaseStatus DatabaseStatus { get; set; }
}
