using Domain.Base;
using Domain.StorageAggregate;
using Infrastructure.Database.Database;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Database.Repository.StorageRepository
{
    public class StorageRepository :RepositoryBase,  IStorageRepository
    {
        public StorageRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {}
        #region Storage
       public async Task<Storage> GetStorageByBlogId(int blogId)
        {
            return  await SpatiumDbContent.Storages.Where(s=> s.BlogId == blogId).Include(s=>s.Folders).ThenInclude(f=>f.Files).FirstOrDefaultAsync();
        }
        #endregion
        #region Folder 

        public async Task CreateFolderAsync(Folder folder)
        {
            await SpatiumDbContent.Folders.AddAsync(folder);
        }

        public async Task DeleteFolderAsync(int folderId)
        {
            var folder = await GetFolderAsync(folderId);
            if (folder == null)
            {
                SpatiumDbContent.Folders.Remove(folder);
            }
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync()
        {
            return await SpatiumDbContent.Folders.ToListAsync();
        }
        public async Task<Folder> GetFolderAsync(int id)
        {
            return await SpatiumDbContent.Folders.FindAsync(id);

        }
        public async Task<Folder> GetFolderAndFileByStorageIdAndFolderId(int storageId, int folderId)
        {
            return await SpatiumDbContent.Folders.Include(f => f.Files).Include(f => f.Folders).FirstOrDefaultAsync(f => f.StorageId == storageId && f.Id == folderId);
          
        }

        public void UpdateFolder(Folder folder)
        {
            SpatiumDbContent.Folders.Update(folder);
        }
        public async Task<bool> ChechNameExists(int blogId, int? parenetId ,string FolderName)
        {
            bool flag;
            //if(parenetId == null)
            //   flag =await SpatiumDbContent.Folders.SingleOrDefaultAsync(f => f.BlogId == blogId && f.ParentId == null && f.Name.ToLower() == FolderName ) is not null? true : false;
            //else
            flag = await SpatiumDbContent.Folders.SingleOrDefaultAsync(f => f.BlogId == blogId && f.ParentId == parenetId && f.Name.ToLower() == FolderName) is not null ? true : false;
            return flag;
        }

        public async Task<Folder> GetFolderByName(string FolderName, int blogId, int? ParentId)
        {
           return await SpatiumDbContent.Folders.SingleOrDefaultAsync(f => f.Name == FolderName&& f.BlogId == blogId && f.ParentId == ParentId);
        }
        #endregion

        #region File
        public async Task CreateFileAsync(StaticFile File)
        {
            await SpatiumDbContent.Files.AddAsync(File);
        }
        public async Task DeleteFileAsync(int FileId)
        {
            var file = await GetFileAsync(FileId);
            if (file is not null)
            {
                SpatiumDbContent.Files.Remove(file);
            }
        }
        public async Task<List<StaticFile>> GetAllFilesAsync(GetEntitiyParams fileParams, int blogId)
        {
            var query = SpatiumDbContent.Files.Where(f => f.BlogId == blogId).AsQueryable();

            if (!string.IsNullOrEmpty(fileParams.FilterColumn) && !string.IsNullOrEmpty(fileParams.FilterValue))
            {
                query = query.ApplyFilter(fileParams.FilterColumn, fileParams.FilterValue);
            }

            if (!string.IsNullOrEmpty(fileParams.SortColumn))
            {
                query = query.ApplySort(fileParams.SortColumn, fileParams.IsDescending);
            }

            if (!string.IsNullOrEmpty(fileParams.SearchColumn) && !string.IsNullOrEmpty(fileParams.SearchValue))
            {
                query = query.ApplySearch(fileParams.SearchColumn, fileParams.SearchValue);
            }

            var paginatedQuery = query.Skip((fileParams.Page - 1) * fileParams.PageSize).Take(fileParams.PageSize);
            return paginatedQuery.ToList();
        }
        public async Task<StaticFile> GetFileAsync(int id)
        {
            return await SpatiumDbContent.Files.FindAsync(id);
        }
        public void UpdateFile(StaticFile File)
        {
            SpatiumDbContent.Files.Update(File);
        }
        #endregion
    }
}
