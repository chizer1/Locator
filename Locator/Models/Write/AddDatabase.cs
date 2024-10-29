using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public class AddDatabase
{
    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string DatabaseUser { get; set; }

    [Required]
    public string DatabaseUserPassword { get; set; }

    [Required]
    public int DatabaseServerId { get; set; }

    [Required]
    public int DatabaseTypeId { get; set; }

    [Required]
    public DatabaseStatus DatabaseStatus { get; set; }
}
