using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public abstract class AddDatabaseServer(string databaseServerName, string databaseServerIpAddress)
{
    [Required]
    public string DatabaseServerName { get; } = databaseServerName;

    [Required]
    public string DatabaseServerIpAddress { get; } = databaseServerIpAddress;
}
