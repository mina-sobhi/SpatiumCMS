
namespace Domain.StorageAggregate
{
    public interface IStorageRepository
    {
        #region Folder
            Task<IEnumerable<Folder>> GetAllFoldersAsync();
            Task<Folder> GetFolderAsync(int id);
            Task CreateFolderAsync(Folder folder);
            Task DeleteFolderAsync(int folderId);
            void UpdateFolder(Folder folder);
        #endregion

        #region Files
                Task<IEnumerable<StaticFile>> GetAllFilesAsync();
                Task<StaticFile> GetFileAsync(int id);
                Task CreateFileAsync(StaticFile File);
                Task DeleteFileAsync(int FileId);
                void UpdateFile(StaticFile File);
        #endregion
    }
}
