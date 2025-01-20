using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Locator.Db;

internal class LocatorContext(DbContextOptions<LocatorContext> options) : DbContext(options)
{
    public virtual DbSet<ClientEntity> Clients { get; set; }

    public virtual DbSet<ClientUserEntity> ClientUsers { get; set; }

    public virtual DbSet<ConnectionEntity> Connections { get; set; }

    public virtual DbSet<DatabaseEntity> Databases { get; set; }

    public virtual DbSet<DatabaseServerEntity> DatabaseServers { get; set; }

    public virtual DbSet<DatabaseTypeEntity> DatabaseTypes { get; set; }

    public virtual DbSet<PermissionEntity> Permissions { get; set; }

    public virtual DbSet<RoleEntity> Roles { get; set; }

    public virtual DbSet<RolePermissionEntity> RolePermissions { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    public virtual DbSet<UserRoleEntity> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientEntity>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK_Client");

            entity.ToTable("Client");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientCode).IsRequired().HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.ClientStatusId).HasColumnName("ClientStatusID");
        });

        modelBuilder.Entity<ClientUserEntity>(entity =>
        {
            entity.HasKey(e => e.ClientUserId).HasName("PK_ClientUser");

            entity.ToTable("ClientUser");

            entity.Property(e => e.ClientUserId).HasColumnName("ClientUserID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity
                .HasOne(d => d.Client)
                .WithMany(p => p.ClientUsers)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUser_Client");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.ClientUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUser_User");
        });

        modelBuilder.Entity<ConnectionEntity>(entity =>
        {
            entity.HasKey(e => e.ConnectionId).HasName("PK_ClientConnection");

            entity.ToTable("Connection");

            entity.HasIndex(e => e.ClientUserId, "ix_ClientConnection_ClientID");

            entity.Property(e => e.ConnectionId).HasColumnName("ConnectionID");
            entity.Property(e => e.ClientUserId).HasColumnName("ClientUserID");
            entity.Property(e => e.DatabaseId).HasColumnName("DatabaseID");

            entity
                .HasOne(d => d.ClientUser)
                .WithMany(p => p.Connections)
                .HasForeignKey(d => d.ClientUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Connection_ClientUser");

            entity
                .HasOne(d => d.Database)
                .WithMany(p => p.Connections)
                .HasForeignKey(d => d.DatabaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Connection_Database");
        });

        modelBuilder.Entity<DatabaseEntity>(entity =>
        {
            entity.HasKey(e => e.DatabaseId).HasName("PK_Database");

            entity.ToTable("Database");

            entity.Property(e => e.DatabaseId).HasColumnName("DatabaseID");
            entity.Property(e => e.DatabaseName).IsRequired().HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.DatabaseServerId).HasColumnName("DatabaseServerID");
            entity.Property(e => e.DatabaseStatusId).HasColumnName("DatabaseStatusID");
            entity.Property(e => e.DatabaseTypeId).HasColumnName("DatabaseTypeID");
            entity.Property(e => e.DatabaseUser).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.DatabaseUserPassword).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.UseTrustedConnection).IsRequired().HasDefaultValue(false);

            entity
                .HasOne(d => d.DatabaseServer)
                .WithMany(p => p.Databases)
                .HasForeignKey(d => d.DatabaseServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Database_DatabaseServer");

            entity
                .HasOne(d => d.DatabaseType)
                .WithMany(p => p.Databases)
                .HasForeignKey(d => d.DatabaseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Database_DatabaseType");
        });

        modelBuilder.Entity<DatabaseServerEntity>(entity =>
        {
            entity.HasKey(e => e.DatabaseServerId).HasName("PK_DatabaseServer");

            entity.ToTable("DatabaseServer");

            entity.Property(e => e.DatabaseServerId).HasColumnName("DatabaseServerID");
            entity
                .Property(e => e.DatabaseServerIpaddress)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DatabaseServerIPAddress");
            entity
                .Property(e => e.DatabaseServerName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DatabaseTypeEntity>(entity =>
        {
            entity.HasKey(e => e.DatabaseTypeId).HasName("PK_DatabaseType");

            entity.ToTable("DatabaseType");

            entity
                .Property(e => e.DatabaseTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DatabaseTypeID");
            entity.Property(e => e.DatabaseTypeName).IsRequired().HasMaxLength(20).IsUnicode(false);
        });

        modelBuilder.Entity<PermissionEntity>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK_Permission");

            entity.ToTable("Permission");

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity
                .Property(e => e.PermissionDescription)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PermissionName).IsRequired().HasMaxLength(50).IsUnicode(false);
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Role_RoleID");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity
                .Property(e => e.Auth0RoleId)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Auth0RoleID");
            entity.Property(e => e.Description).IsRequired().HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(20).IsUnicode(false);
        });

        modelBuilder.Entity<RolePermissionEntity>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK_RolePermission");

            entity.ToTable("RolePermission");

            entity.Property(e => e.RolePermissionId).HasColumnName("RolePermissionID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity
                .HasOne(d => d.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity
                .HasOne(d => d.Role)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User_UserID");

            entity.ToTable("User", tb => tb.HasTrigger("User_ClientID_Update_Prevention"));

            entity.HasIndex(e => e.Auth0Id, "ix_User_Auth0ID").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity
                .Property(e => e.Auth0Id)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Auth0ID");
            entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.UserStatusId).HasColumnName("UserStatusID");
        });

        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK_UserRole");

            entity.ToTable("UserRole");

            entity
                .HasIndex(e => new { e.UserId, e.RoleId }, "uix_UserRole_UserID_RoleID")
                .IsUnique();

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity
                .HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");
        });
    }
}
