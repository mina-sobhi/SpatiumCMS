using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class FolderHierarcyCTE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                            CREATE FUNCTION [dbo].[FolderAndChild](@FolderId int,@BlogId int)

                            RETURNS TABLE

                            AS

                            RETURN

                            (

                            WITH  h AS(

                            select * from Folders  

                            where Id=@FolderId AND BlogId=@BlogId

                            union all

                            select subfolders.*

                            from Folders subFolders

                            JOIN h

                            on subFolders.ParentId= h.Id 

                            where  subFolders.IsDeleted=0

                            )

                            select * from h

                            );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
