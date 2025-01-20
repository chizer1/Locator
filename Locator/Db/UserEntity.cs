namespace Locator.Db;

internal class UserEntity
{
    public int UserId { get; set; }

    public string Auth0Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public byte UserStatusId { get; set; }

    public virtual ICollection<ClientUserEntity> ClientUsers { get; set; } = [];

    public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = [];
}
