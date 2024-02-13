using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UserRolesPermissionsModulesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [SpatiumCMS]
                    GO
                    INSERT [dbo].[AspNetRoles] ([Id], [IconPath], [Description], [IsActive], [RoleOwnerId], [Name], [NormalizedName], [ConcurrencyStamp], [Priority]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', NULL, NULL, 1, NULL, N'Super Admin', N'SUPER ADMIN', NULL, 1)
                    GO
                    INSERT [dbo].[AspNetRoles] ([Id], [IconPath], [Description], [IsActive], [RoleOwnerId], [Name], [NormalizedName], [ConcurrencyStamp], [Priority]) VALUES (N'5c78edbb-0121-4a88-a7b1-5172d77e2aed', NULL, NULL, 1, NULL, N'Unassigned', N'UNASSIGNED', NULL, 2)
                    GO
                    INSERT [dbo].[AspNetRoles] ([Id], [IconPath], [Description], [IsActive], [RoleOwnerId], [Name], [NormalizedName], [ConcurrencyStamp], [Priority]) VALUES (N'8a3f1c05-9db4-447a-989c-d4db082fb6ca', NULL, NULL, 1, NULL, N'Article Creator', N'ARTICLE CREATOR', NULL, 2)
                    GO
                    INSERT [dbo].[AspNetRoles] ([Id], [IconPath], [Description], [IsActive], [RoleOwnerId], [Name], [NormalizedName], [ConcurrencyStamp], [Priority]) VALUES (N'93b6c7df-b613-4b7f-a793-afc260d0a5a0', NULL, NULL, 1, NULL, N'Editor', N'EDITOR', NULL, 2)
                    GO
                    INSERT [dbo].[AspNetRoles] ([Id], [IconPath], [Description], [IsActive], [RoleOwnerId], [Name], [NormalizedName], [ConcurrencyStamp], [Priority]) VALUES (N'97152995-238d-44a2-a727-f75b0c530f0c', NULL, NULL, 1, NULL, N'Viewer', N'VIEWER', NULL, 2)
                    GO
                    INSERT [dbo].[AspNetRoles] ([Id], [IconPath], [Description], [IsActive], [RoleOwnerId], [Name], [NormalizedName], [ConcurrencyStamp], [Priority]) VALUES (N'b62e1554-257e-4e83-94df-9d56e8b27367', NULL, NULL, 1, NULL, N'SEO Specialist', N'SEO SPECIALIST', NULL, 2)
                    GO


                    SET IDENTITY_INSERT [dbo].[UserModules] ON 
                    GO
                    INSERT [dbo].[UserModules] ([Id], [Name]) VALUES (1, N'Blog Posts Management')
                    GO
                    INSERT [dbo].[UserModules] ([Id], [Name]) VALUES (2, N'Media Management')
                    GO
                    INSERT [dbo].[UserModules] ([Id], [Name]) VALUES (3, N'SEO Management')
                    GO
                    INSERT [dbo].[UserModules] ([Id], [Name]) VALUES (4, N'Reports & Analytics')
                    GO
                    INSERT [dbo].[UserModules] ([Id], [Name]) VALUES (5, N'Comments')
                    GO
                    INSERT [dbo].[UserModules] ([Id], [Name]) VALUES (6, N'Users Management')
                    GO
                    SET IDENTITY_INSERT [dbo].[UserModules] OFF
                    GO
                    SET IDENTITY_INSERT [dbo].[UserPermissions] ON 
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (1, N'Create Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (2, N'Read Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (3, N'Update Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (4, N'Delete Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (5, N'Export Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (6, N'Import Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (7, N'Publish Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (8, N'Unpublish Posts', 1)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (100, N'Create Users', 6)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (101, N'Read Users', 6)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (102, N'Update Users', 6)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (103, N'Delete Users', 6)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (104, N'Import Users', 6)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (105, N'Export Users', 6)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (200, N'Create Media', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (201, N'Read Media', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (202, N'Update Media', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (203, N'Delete Media', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (204, N'Import Media', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (205, N'Export Media', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (206, N'Edit Media Meta Information', 2)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (300, N'Create SEO', 3)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (301, N'Read SEO', 3)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (302, N'Update SEO', 3)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (303, N'Delete SEO', 3)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (304, N'Import SEO', 3)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (305, N'Export SEO', 3)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (400, N'Create Comment', 5)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (401, N'Read Comment', 5)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (402, N'Update Comment', 5)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (403, N'Delete Comment', 5)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (404, N'Import Comment', 5)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (405, N'Export Comment', 5)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (500, N'Create Report', 4)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (501, N'Read Report', 4)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (502, N'Update Report', 4)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (503, N'Delete Report', 4)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (504, N'Import Report', 4)
                    GO
                    INSERT [dbo].[UserPermissions] ([Id], [Name], [UserModuleId]) VALUES (505, N'Export Report', 4)
                    GO
                    SET IDENTITY_INSERT [dbo].[UserPermissions] OFF
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 1)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'8a3f1c05-9db4-447a-989c-d4db082fb6ca', 1)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 2)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'8a3f1c05-9db4-447a-989c-d4db082fb6ca', 2)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'93b6c7df-b613-4b7f-a793-afc260d0a5a0', 2)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'97152995-238d-44a2-a727-f75b0c530f0c', 2)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'b62e1554-257e-4e83-94df-9d56e8b27367', 2)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 3)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'8a3f1c05-9db4-447a-989c-d4db082fb6ca', 3)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'93b6c7df-b613-4b7f-a793-afc260d0a5a0', 3)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'b62e1554-257e-4e83-94df-9d56e8b27367', 3)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 4)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'8a3f1c05-9db4-447a-989c-d4db082fb6ca', 4)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 5)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 6)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 7)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 8)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 100)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 101)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 102)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 103)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 104)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 105)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 200)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 201)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'97152995-238d-44a2-a727-f75b0c530f0c', 201)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 202)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 203)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 204)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 205)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 206)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'b62e1554-257e-4e83-94df-9d56e8b27367', 206)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 300)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 301)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 302)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 303)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 304)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 305)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 400)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 401)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 402)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 403)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 404)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 405)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 500)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 501)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'97152995-238d-44a2-a727-f75b0c530f0c', 501)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'b62e1554-257e-4e83-94df-9d56e8b27367', 501)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 502)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 503)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 504)
                    GO
                    INSERT [dbo].[RolePermission] ([UserRoleId], [UserPermissionId]) VALUES (N'10def0fc-dbe7-4807-b01c-9d75dfdc67de', 505)
                    GO

                        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //To be implemented to remove the static data
        }
    }
}
