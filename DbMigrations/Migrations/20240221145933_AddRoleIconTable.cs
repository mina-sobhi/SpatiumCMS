using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleIconTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconPath",
                table: "AspNetRoles",
                newName: "Color");

            migrationBuilder.AddColumn<int>(
                name: "RoleIconId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleIcon",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IconPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleIcon", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_RoleIconId",
                table: "AspNetRoles",
                column: "RoleIconId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_RoleIcon_RoleIconId",
                table: "AspNetRoles",
                column: "RoleIconId",
                principalSchema: "Lookup",
                principalTable: "RoleIcon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_RoleIcon_RoleIconId",
                table: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "RoleIcon",
                schema: "Lookup");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_RoleIconId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "RoleIconId",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "AspNetRoles",
                newName: "IconPath");
        }
    }
}
