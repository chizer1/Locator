using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class UpdateDatabase(
    int databaseId,
    string databaseName,
    string databaseUserName,
    int databaseServerId,
    int databaseTypeId,
    DatabaseStatus databaseStatus
)
{
    [Required]
    public int DatabaseId { get; } = databaseId;

    [Required]
    public string DatabaseName { get; } = databaseName;

    [Required]
    public string DatabaseUserName { get; } = databaseUserName;

    [Required]
    public int DatabaseServerId { get; } = databaseServerId;

    [Required]
    public int DatabaseTypeId { get; } = databaseTypeId;

    [Required]
    public DatabaseStatus DatabaseStatus { get; } = databaseStatus;
}
