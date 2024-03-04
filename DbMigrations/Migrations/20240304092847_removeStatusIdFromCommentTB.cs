using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class removeStatusIdFromCommentTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommentStatus_CommentStatusId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentStatusId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentStatusId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StatusId",
                table: "Comments",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommentStatus_StatusId",
                table: "Comments",
                column: "StatusId",
                principalSchema: "Lookup",
                principalTable: "CommentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommentStatus_StatusId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_StatusId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "CommentStatusId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentStatusId",
                table: "Comments",
                column: "CommentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommentStatus_CommentStatusId",
                table: "Comments",
                column: "CommentStatusId",
                principalSchema: "Lookup",
                principalTable: "CommentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
