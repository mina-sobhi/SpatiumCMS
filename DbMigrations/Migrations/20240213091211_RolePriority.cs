using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RolePriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
            //Updating the priority of Main Roles
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                                GO
                                Update [dbo].[AspNetRoles] set [Priority] = 1 where [Id] =N'10def0fc-dbe7-4807-b01c-9d75dfdc67de'
                                GO
                                Update [dbo].[AspNetRoles] set [Priority] = 2 where [Id]= N'5c78edbb-0121-4a88-a7b1-5172d77e2aed'
                                GO
                                update [dbo].[AspNetRoles] set [Priority] = 2 where [Id]= N'8a3f1c05-9db4-447a-989c-d4db082fb6ca'
                                GO
                                update [dbo].[AspNetRoles] set [Priority] = 2 where [Id]= N'93b6c7df-b613-4b7f-a793-afc260d0a5a0'
                                GO
                                update [dbo].[AspNetRoles] set [Priority] = 2 where [Id]= N'97152995-238d-44a2-a727-f75b0c530f0c'
                                GO
                                update [dbo].[AspNetRoles] set [Priority] = 2 where [Id]=N'b62e1554-257e-4e83-94df-9d56e8b27367'
                                GO
                                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "AspNetRoles");
        }
    }
}
