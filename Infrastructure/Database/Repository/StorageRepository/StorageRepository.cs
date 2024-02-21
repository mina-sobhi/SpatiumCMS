using Domain.StorageAggregate;
using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Database.Repository.StorageRepository
{
    public class StorageRepository :RepositoryBase,  IStorageRepository
    {
        public StorageRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
        {}
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
        public async Task<IEnumerable<StaticFile>> GetAllFilesAsync()
        {
            return await SpatiumDbContent.Files.ToListAsync();
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
