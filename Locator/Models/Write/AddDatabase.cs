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
    public string DatabaseName { get; set; } = databaseName;

    [Required]
    public string DatabaseUser { get; set; } = databaseUser;

    [Required]
    public int DatabaseServerId { get; set; } = databaseServerId;

    [Required]
    public int DatabaseTypeId { get; set; } = databaseTypeId;

    [Required]
    public DatabaseStatus DatabaseStatus { get; set; } = databaseStatus;
}
