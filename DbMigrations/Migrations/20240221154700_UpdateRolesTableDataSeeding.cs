using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRolesTableDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"use SpatiumCMS
                         GO
                         UPDATE AspNetRoles
                         SET RoleIconId = 1
                         WHERE ID='10def0fc-dbe7-4807-b01c-9d75dfdc67de';
                         GO
                         UPDATE AspNetRoles
                         SET RoleIconId = 2
                         WHERE ID='93b6c7df-b613-4b7f-a793-afc260d0a5a0';
                         GO
                         UPDATE AspNetRoles
                         SET RoleIconId = 3
                         WHERE ID='97152995-238d-44a2-a727-f75b0c530f0c';
                         GO
                         UPDATE AspNetRoles
                         SET RoleIconId = 4
                         WHERE ID='8a3f1c05-9db4-447a-989c-d4db082fb6ca';
                         GO
                         UPDATE AspNetRoles
                         SET RoleIconId = 5
                         WHERE ID='b62e1554-257e-4e83-94df-9d56e8b27367';
                         GO
                         UPDATE AspNetRoles
                         SET RoleIconId = 6
                         WHERE ID='5c78edbb-0121-4a88-a7b1-5172d77e2aed';
                         GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
