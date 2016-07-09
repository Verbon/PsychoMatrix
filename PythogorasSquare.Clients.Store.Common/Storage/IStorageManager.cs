using System;
using System.Threading.Tasks;
using Windows.Storage;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.Store.Common.Storage
{
    public interface IStorageManager
    {
        Task<bool> CheckIfCanAccessFileAsync(string path);

        string GetStorageItemFutureAccessToken(IStorageItem storageItem);

        Task<OperationResult<IStorageFolder>> GetSavedForFutureAccessFolderAsync(string token);

        Task<OperationResult<IStorageFile>> TryGetFileAsync(string path);

        Task<OperationResult<IStorageFile>> TryGetFileAsync(Uri uri);

        Task<bool> DeleteFileAsync(string path, bool shouldDeletePermanently = false);

        string ResolveInvalidPath(string path, string replaceWith);

        Task<ulong> GetFreeSpaceAsync(IStorageFolder storageFolder);
    }
}