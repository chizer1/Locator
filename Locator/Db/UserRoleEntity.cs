namespace Locator.Db;

internal class UserRoleEntity
{
    public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; }

    public virtual UserEntity User { get; set; }
}
