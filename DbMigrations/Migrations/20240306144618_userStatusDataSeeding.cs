using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class userStatusDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                                GO
                                SET IDENTITY_INSERT [Lookup].[UserStatus] ON 
                                GO
                                INSERT [Lookup].[UserStatus] ([Id], [Name]) VALUES (1, N'Active')
                                GO
                                INSERT [Lookup].[UserStatus] ([Id], [Name]) VALUES (2, N'DeActive')
                                GO
                                INSERT [Lookup].[UserStatus] ([Id], [Name]) VALUES (3, N'Pending')
                                GO
                                SET IDENTITY_INSERT [Lookup].[PostStatus] OFF
                                GO
                                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
