namespace Locator.Db;

internal class ConnectionEntity
{
    public int ConnectionId { get; set; }

    public int ClientUserId { get; set; }

    public int DatabaseId { get; set; }

    public virtual ClientUserEntity ClientUser { get; set; }

    public virtual DatabaseEntity Database { get; set; }
}
