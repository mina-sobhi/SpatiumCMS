using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RefactoryShareTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_AspNetUsers_CreatedbyId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Posts_PostId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Share_AspNetUsers_CreatedbyId",
                table: "Share");

            migrationBuilder.DropForeignKey(
                name: "FK_Share_Posts_PostId",
                table: "Share");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Share",
                table: "Share");

            migrationBuilder.DropIndex(
                name: "IX_Share_CreatedbyId",
                table: "Share");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "CreatedbyId",
                table: "Share");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Share");

            migrationBuilder.DropColumn(
                name: "IsShare",
                table: "Share");

            migrationBuilder.RenameTable(
                name: "Share",
                newName: "Shares");

            migrationBuilder.RenameTable(
                name: "Like",
                newName: "Likes");

            migrationBuilder.RenameIndex(
                name: "IX_Share_PostId",
                table: "Shares",
                newName: "IX_Shares_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_PostId",
                table: "Likes",
                newName: "IX_Likes_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_CreatedbyId",
                table: "Likes",
                newName: "IX_Likes_CreatedbyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shares",
                table: "Shares",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_CreatedbyId",
                table: "Likes",
                column: "CreatedbyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Posts_PostId",
                table: "Shares",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_CreatedbyId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Posts_PostId",
                table: "Shares");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shares",
                table: "Shares");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Shares",
                newName: "Share");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "Like");

            migrationBuilder.RenameIndex(
                name: "IX_Shares_PostId",
                table: "Share",
                newName: "IX_Share_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PostId",
                table: "Like",
                newName: "IX_Like_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_CreatedbyId",
                table: "Like",
                newName: "IX_Like_CreatedbyId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedbyId",
                table: "Share",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Share",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShare",
                table: "Share",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Share",
                table: "Share",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Share_CreatedbyId",
                table: "Share",
                column: "CreatedbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_AspNetUsers_CreatedbyId",
                table: "Like",
                column: "CreatedbyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Posts_PostId",
                table: "Like",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Share_AspNetUsers_CreatedbyId",
                table: "Share",
                column: "CreatedbyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Share_Posts_PostId",
                table: "Share",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
