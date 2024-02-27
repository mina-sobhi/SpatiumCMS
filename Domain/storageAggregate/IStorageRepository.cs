
using Domain.Base;

namespace Domain.StorageAggregate
{
    public interface IStorageRepository
    {
        #region Storage
        Task<Storage> GetStorageByBlogId(int blogId);
        Task AddStorage (Storage storage);
        #endregion

        #region Folder
        Task<IEnumerable<Folder>> GetAllFoldersAsync();
        Task<Folder> GetFolderAsync(int id);
        Task<Folder> GetFolderAndFileByStorageIdAndFolderId(int storageId, int folderId);
        Task CreateFolderAsync(Folder folder);
        Task DeleteFolderAsync(int folderId);
        void UpdateFolder(Folder folder);
        Task<bool> ChechNameExists(int blogId, int? parenetId , string FolderName);
        Task<Folder> GetFolderByName(string FolderName , int blogId , int? ParentId);
        #endregion

        #region Files
        Task<List<StaticFile>> GetAllFilesAsync(GetEntitiyParams fileParams, int blogId);
        Task<StaticFile> GetFileAsync(int id);
        Task CreateFileAsync(StaticFile File);
        Task DeleteFileAsync(int FileId);
        void UpdateFile(StaticFile File);
        Task<Folder> GetFilesToExtract(int bloId,int? folderId);

        #endregion
    }
}
