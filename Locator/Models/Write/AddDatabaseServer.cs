using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public class AddDatabaseServer
{
    [Required]
    public string DatabaseServerName { get; set; }

    [Required]
    public string DatabaseServerIpAddress { get; set; }
}
