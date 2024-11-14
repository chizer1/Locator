using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public abstract class UpdateDatabaseServer(
    int databaseServerId,
    string databaseServerName,
    string databaseServerIpAddress
)
{
    [Required]
    public int DatabaseServerId { get; } = databaseServerId;

    [Required]
    public string DatabaseServerName { get; } = databaseServerName;

    [Required]
    public string DatabaseServerIpAddress { get; } = databaseServerIpAddress;
}
