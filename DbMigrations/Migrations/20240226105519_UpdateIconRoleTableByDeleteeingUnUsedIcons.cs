using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIconRoleTableByDeleteeingUnUsedIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"use SpatiumCMS
                                GO
                                DELETE FROM [Lookup].[RoleIcon]  WHERE id=1;
                                GO
                                DELETE FROM [Lookup].[RoleIcon]  WHERE id=2;
                                GO
                                DELETE FROM [Lookup].[RoleIcon]  WHERE id=3;
                                GO
                                DELETE FROM [Lookup].[RoleIcon]  WHERE id=4;
                                GO
                                DELETE FROM [Lookup].[RoleIcon]  WHERE id=5;
                                GO
                                DELETE FROM [Lookup].[RoleIcon]  WHERE id=6;
                                GO
                                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
