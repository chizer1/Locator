using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public class UpdateDatabaseServer
{
    [Required]
    public int DatabaseServerId { get; set; }

    [Required]
    public string DatabaseServerName { get; set; }

    [Required]
    public string DatabaseServerIpAddress { get; set; }
}
