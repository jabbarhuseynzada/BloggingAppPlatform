using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class folloTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersFollower",
                table: "UsersFollower");

            migrationBuilder.RenameTable(
                name: "UsersFollower",
                newName: "UserFollower");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollower",
                table: "UserFollower",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollower",
                table: "UserFollower");

            migrationBuilder.RenameTable(
                name: "UserFollower",
                newName: "UsersFollower");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersFollower",
                table: "UsersFollower",
                column: "Id");
        }
    }
}
