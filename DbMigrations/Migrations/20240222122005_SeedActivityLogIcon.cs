using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SeedActivityLogIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                    GO
                    INSERT [dbo].[LogIcons] ( [Path],[Name]) VALUES ('ActivityLog/assigned.svg','Assinge')
                    GO
                    INSERT [dbo].[LogIcons] ( [Path],[Name]) VALUES ('ActivityLog/comment.svg','Comment')
                    GO
                    INSERT [dbo].[LogIcons] ( [Path],[Name]) VALUES ('ActivityLog/new.svg','New')
                    GO
              ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
