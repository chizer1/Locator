using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locator.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DatabaseUser",
                table: "Database",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50
            );

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseName",
                table: "Database",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50
            );

            migrationBuilder.AddColumn<string>(
                name: "DatabaseUserPassword",
                table: "Database",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "UseTrustedConnection",
                table: "Database",
                type: "bit",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DatabaseUserPassword", table: "Database");

            migrationBuilder.DropColumn(name: "UseTrustedConnection", table: "Database");

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseUser",
                table: "Database",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseName",
                table: "Database",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true
            );
        }
    }
}
