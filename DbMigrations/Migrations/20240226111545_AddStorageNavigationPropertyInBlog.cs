using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageNavigationPropertyInBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Storages_BlogId",
                table: "Storages");

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Storages_BlogId",
                table: "Storages",
                column: "BlogId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Storages_BlogId",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "Blogs");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_BlogId",
                table: "Storages",
                column: "BlogId");
        }
    }
}
