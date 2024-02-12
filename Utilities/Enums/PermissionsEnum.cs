namespace Utilities.Enums
{
    public enum PermissionsEnum
    {
        #region PostPermissions
        CreatePost = 1,
        ReadPost = 2,
        UpdatePost = 3,
        DeletePost = 4,
        ImportPost = 5,
        ExportPost = 6,
        PublishPost = 7,
        UnpublishPost = 8,
        #endregion

        #region UserPermissions
        CreateUser= 100,
        ReadUser= 101,
        UpdateUser= 102,
        DeleteUser= 103,
        ImportUser= 104,
        ExportUser=105,
        #endregion

        #region MediaManagementPermissions
        CreateMedia= 200,
        ReadMedia= 201,
        UpdateMedia= 202,
        DeleteMedia= 203,
        ImportMedia= 204,
        ExportMedia= 205,
        EditMediaMetaInformation = 206,
        #endregion

        #region SEO
        CreateSEO=300,
        ReadSEO=301,
        UpdateSEO=302,
        DeleteSEO=303,
        ImportSEO=304,
        ExportSEO=305,
        #endregion

        #region Comments
        CreateComment=400,
        ReadComment=401,
        UpdateComment=402,
        DeleteComment=403,
        ImportComment=404,
        ExportComment=405,
        #endregion

        #region Reports
        CreateReport=500,
        ReadReport=501,
        UpdateReport=502,
        DeleteReport=503,
        ImportReport=504,
        ExportReport=505,
        #endregion
    }
}
