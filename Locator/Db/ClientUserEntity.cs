namespace Locator.Db;

internal class ClientUserEntity
{
    public int ClientUserId { get; set; }

    public int ClientId { get; set; }

    public int UserId { get; set; }

    public virtual ClientEntity Client { get; set; }

    public virtual ICollection<ConnectionEntity> Connections { get; set; } = [];

    public virtual UserEntity User { get; set; }
}
