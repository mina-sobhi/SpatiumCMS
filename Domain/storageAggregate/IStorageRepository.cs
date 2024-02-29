
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
        Task<Folder> GetFolderAsync(int folderId, int blogId);
        Task<Folder> GetFolderAndFileByStorageIdAndFolderId(int storageId, int folderId,int blogId);
        Task CreateFolderAsync(Folder folder);
        Task DeleteFolderAsync(int id, int blogId);
        void UpdateFolder(Folder folder);
        Task<bool> ChechNameExists(int blogId, int? parenetId , string FolderName);
        Task<Folder> GetFolderByName(string FolderName , int blogId , int? ParentId);
        #endregion

        #region Files
        Task<List<StaticFile>> GetAllFilesAsync(GetEntitiyParams fileParams, int blogId);
        Task<StaticFile> GetFileAsync(int id,int blogId);
        Task CreateFileAsync(StaticFile File);
        Task DeleteFileAsync(int FileId, int blogId);
        void UpdateFile(StaticFile File);
        Task<Folder> GetFilesToExtract(int bloId,int? folderId);
        Task<bool> ChechFileNameExists(string FileName);
        Task<bool> ChechFolderExists(int? Id);

        #endregion
    }
}
