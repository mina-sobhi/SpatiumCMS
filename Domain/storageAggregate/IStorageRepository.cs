﻿
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
        Task<bool> ChechNameExists(int blogId, int? parenetId , string FolderName);
        Task<Folder> GetFolderByName(string FolderName , int blogId , int? ParentId);
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
