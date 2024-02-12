using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddPostStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                            GO
                            SET IDENTITY_INSERT [Lookup].[PostStatus] ON 
                            GO
                            INSERT [Lookup].[PostStatus] ([Id], [Name]) VALUES (1, N'Published')
                            GO
                            INSERT [Lookup].[PostStatus] ([Id], [Name]) VALUES (2, N'Unpublished')
                            GO
                            INSERT [Lookup].[PostStatus] ([Id], [Name]) VALUES (3, N'Pending')
                            GO
                            INSERT [Lookup].[PostStatus] ([Id], [Name]) VALUES (4, N'Scheduled')
                            GO
                            INSERT [Lookup].[PostStatus] ([Id], [Name]) VALUES (5, N'Draft')
                            GO
                            SET IDENTITY_INSERT [Lookup].[PostStatus] OFF
                            GO
                            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove ids
        }
    }
}
