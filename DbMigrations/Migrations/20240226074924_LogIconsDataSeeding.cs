using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class LogIconsDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                    GO
                    INSERT [Lookup].[LogIcons] ( [Path],[Name]) VALUES ('ActivityLog/assigned.svg','Assinge')
                    GO
                    INSERT [Lookup].[LogIcons] ( [Path],[Name]) VALUES ('ActivityLog/comment.svg','Comment')
                    GO
                    INSERT [Lookup].[LogIcons] ( [Path],[Name]) VALUES ('ActivityLog/new.svg','New')
                    GO
              ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
