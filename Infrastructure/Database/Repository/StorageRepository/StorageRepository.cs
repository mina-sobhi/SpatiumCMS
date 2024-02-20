using Domain.storageAggregate;
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
