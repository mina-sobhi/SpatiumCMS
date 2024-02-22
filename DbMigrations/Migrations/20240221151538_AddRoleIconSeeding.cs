using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleIconSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                                    GO
                                    SET IDENTITY_INSERT [Lookup].[RoleIcon] ON 
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (1, N'RoleIcon/SuperAdmin.svg', N'SuperAdmin')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (2, N'RoleIcon/Viewer.svg', N'Viewer')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (3, N'RoleIcon/Editor.svg', N'Editor')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (4, N'RoleIcon/Articale Creator.svg', N'Articale Creator')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (5, N'RoleIcon/SEO Specialist.svg', N'SEO Specialist')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (6, N'RoleIcon/Unassigned.svg', N'Unassigned')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (7, N'RoleIcon/3d-cube-scan.svg', N'3d-cube-scan')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (8, N'RoleIcon/abstract-32.svg', N'abstract-32')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (9, N'RoleIcon/brifecase-timer.svg', N'brifecase-timer')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (10, N'RoleIcon/building-4.svg', N'building-4')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (11, N'RoleIcon/calculator.svg', N'calculator')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (12, N'RoleIcon/chart.svg', N'chart')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (13, N'RoleIcon/check.svg', N'check')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (14, N'RoleIcon/code.svg', N'code')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (15, N'RoleIcon/eye.svg', N'eye')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (16, N'RoleIcon/monitor-mobbile.svg', N'monitor-mobbile')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (17, N'RoleIcon/note-2.svg', N'note-2')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (18, N'RoleIcon/pencxil.svg', N'pencxil')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (19, N'RoleIcon/security-time.svg', N'security-time')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (20, N'RoleIcon/status-up.svg', N'status-up')
                                    GO
                                    INSERT [Lookup].[RoleIcon] ([Id], [IconPath], [Name]) VALUES (21, N'RoleIcon/user-edit', N'user-edit')
                                    GO
                                    SET IDENTITY_INSERT [Lookup].[RoleIcon] OFF
                                    GO
                                    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
