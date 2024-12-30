namespace Locator.Db;

internal class ClientEntity
{
    public int ClientId { get; set; }

    public string ClientName { get; set; }

    public string ClientCode { get; set; }

    public int ClientStatusId { get; set; }

    public virtual ICollection<ClientUserEntity> ClientUsers { get; set; } = [];
}
