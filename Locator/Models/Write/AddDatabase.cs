using System.ComponentModel.DataAnnotations;
using Locator.Models.Read;

namespace Locator.Models.Write;

public abstract class AddDatabase(
    string databaseName,
    string databaseUser,
    int databaseServerId,
    int databaseTypeId,
    DatabaseStatus databaseStatus
)
{
    [Required]
    public string DatabaseName { get; } = databaseName;

    [Required]
    public string DatabaseUser { get; } = databaseUser;

    [Required]
    public int DatabaseServerId { get; } = databaseServerId;

    [Required]
    public int DatabaseTypeId { get; } = databaseTypeId;

    [Required]
    public DatabaseStatus DatabaseStatus { get; } = databaseStatus;
}
