using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locator.Migrations
{
    /// <inheritdoc />
    internal partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    ClientCode = table.Column<string>(
                        type: "varchar(20)",
                        unicode: false,
                        maxLength: 20,
                        nullable: false
                    ),
                    ClientStatusID = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientID);
                }
            );

            migrationBuilder.CreateTable(
                name: "DatabaseServer",
                columns: table => new
                {
                    DatabaseServerID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseServerName = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    DatabaseServerIPAddress = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseServer", x => x.DatabaseServerID);
                }
            );

            migrationBuilder.CreateTable(
                name: "DatabaseType",
                columns: table => new
                {
                    DatabaseTypeID = table
                        .Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseTypeName = table.Column<string>(
                        type: "varchar(20)",
                        unicode: false,
                        maxLength: 20,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseType", x => x.DatabaseTypeID);
                }
            );

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    PermissionDescription = table.Column<string>(
                        type: "varchar(100)",
                        unicode: false,
                        maxLength: 100,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionID);
                }
            );

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Auth0RoleID = table.Column<string>(
                        type: "varchar(20)",
                        unicode: false,
                        maxLength: 20,
                        nullable: false
                    ),
                    Name = table.Column<string>(
                        type: "varchar(20)",
                        unicode: false,
                        maxLength: 20,
                        nullable: false
                    ),
                    Description = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_RoleID", x => x.RoleID);
                }
            );

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Auth0ID = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    FirstName = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    LastName = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    EmailAddress = table.Column<string>(
                        type: "varchar(100)",
                        unicode: false,
                        maxLength: 100,
                        nullable: false
                    ),
                    UserStatusID = table.Column<byte>(type: "tinyint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_UserID", x => x.UserID);
                }
            );

            migrationBuilder.CreateTable(
                name: "Database",
                columns: table => new
                {
                    DatabaseID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseName = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    DatabaseUser = table.Column<string>(
                        type: "varchar(50)",
                        unicode: false,
                        maxLength: 50,
                        nullable: false
                    ),
                    DatabaseServerID = table.Column<int>(type: "int", nullable: false),
                    DatabaseTypeID = table.Column<byte>(type: "tinyint", nullable: false),
                    DatabaseStatusID = table.Column<byte>(type: "tinyint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Database", x => x.DatabaseID);
                    table.ForeignKey(
                        name: "FK_Database_DatabaseServer",
                        column: x => x.DatabaseServerID,
                        principalTable: "DatabaseServer",
                        principalColumn: "DatabaseServerID"
                    );
                    table.ForeignKey(
                        name: "FK_Database_DatabaseType",
                        column: x => x.DatabaseTypeID,
                        principalTable: "DatabaseType",
                        principalColumn: "DatabaseTypeID"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RolePermissionID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.RolePermissionID);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "PermissionID"
                    );
                    table.ForeignKey(
                        name: "FK_RolePermission_Role",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ClientUser",
                columns: table => new
                {
                    ClientUserID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUser", x => x.ClientUserID);
                    table.ForeignKey(
                        name: "FK_ClientUser_Client",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ClientID"
                    );
                    table.ForeignKey(
                        name: "FK_ClientUser_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserRoleID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleID);
                    table.ForeignKey(
                        name: "FK_UserRole_Role",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID"
                    );
                    table.ForeignKey(
                        name: "FK_UserRole_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Connection",
                columns: table => new
                {
                    ConnectionID = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientUserID = table.Column<int>(type: "int", nullable: false),
                    DatabaseID = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientConnection", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "FK_Connection_ClientUser",
                        column: x => x.ClientUserID,
                        principalTable: "ClientUser",
                        principalColumn: "ClientUserID"
                    );
                    table.ForeignKey(
                        name: "FK_Connection_Database",
                        column: x => x.DatabaseID,
                        principalTable: "Database",
                        principalColumn: "DatabaseID"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_ClientID",
                table: "ClientUser",
                column: "ClientID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_UserID",
                table: "ClientUser",
                column: "UserID"
            );

            migrationBuilder.CreateIndex(
                name: "ix_ClientConnection_ClientID",
                table: "Connection",
                column: "ClientUserID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Connection_DatabaseID",
                table: "Connection",
                column: "DatabaseID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Database_DatabaseServerID",
                table: "Database",
                column: "DatabaseServerID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Database_DatabaseTypeID",
                table: "Database",
                column: "DatabaseTypeID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionID",
                table: "RolePermission",
                column: "PermissionID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleID",
                table: "RolePermission",
                column: "RoleID"
            );

            migrationBuilder.CreateIndex(
                name: "ix_User_Auth0ID",
                table: "User",
                column: "Auth0ID",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleID",
                table: "UserRole",
                column: "RoleID"
            );

            migrationBuilder.CreateIndex(
                name: "uix_UserRole_UserID_RoleID",
                table: "UserRole",
                columns: new[] { "UserID", "RoleID" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Connection");

            migrationBuilder.DropTable(name: "RolePermission");

            migrationBuilder.DropTable(name: "UserRole");

            migrationBuilder.DropTable(name: "ClientUser");

            migrationBuilder.DropTable(name: "Database");

            migrationBuilder.DropTable(name: "Permission");

            migrationBuilder.DropTable(name: "Role");

            migrationBuilder.DropTable(name: "Client");

            migrationBuilder.DropTable(name: "User");

            migrationBuilder.DropTable(name: "DatabaseServer");

            migrationBuilder.DropTable(name: "DatabaseType");
        }
    }
}
