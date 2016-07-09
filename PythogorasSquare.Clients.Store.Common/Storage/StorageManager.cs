using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Extensions;

namespace PythogorasSquare.Clients.Store.Common.Storage
{
    [UsedImplicitly]
    public class StorageManager : IStorageManager
    {
        private const string MsAppxScheme = "ms-appx";


        public async Task<bool> CheckIfCanAccessFileAsync(string path)
        {
            var getFileResult = await TryGetFileAsync(path).ConfigureAwait(false);

            return getFileResult.IsSuccessful;
        }

        public string GetStorageItemFutureAccessToken(IStorageItem storageItem)
            => StorageApplicationPermissions.FutureAccessList.Add(storageItem);

        public async Task<OperationResult<IStorageFolder>> GetSavedForFutureAccessFolderAsync(string token)
        {
            try
            {
                return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            }
            catch (Exception ex)
            {
                if (!ex.IsCatchableExceptionType())
                {
                    throw;
                }

                return OperationResult<IStorageFolder>.CreateUnsuccessful();
            }
        }

        public async Task<OperationResult<IStorageFile>> TryGetFileAsync(string path)
        {
            try
            {
                if (CheckIfPathIsUri(path))
                {
                    return await TryGetFileAsync(new Uri(path));
                }

                return await StorageFile.GetFileFromPathAsync(path);
            }
            catch (Exception ex)
            {
                if (!ex.IsCatchableExceptionType())
                {
                    throw;
                }

                return OperationResult<IStorageFile>.CreateUnsuccessful();
            }
        }

        public async Task<OperationResult<IStorageFile>> TryGetFileAsync(Uri uri)
        {
            try
            {
                return await StorageFile.GetFileFromApplicationUriAsync(uri);
            }
            catch (Exception ex)
            {
                if (!ex.IsCatchableExceptionType())
                {
                    throw;
                }

                return OperationResult<IStorageFile>.CreateUnsuccessful();
            }
        }

        public async Task<bool> DeleteFileAsync(string path, bool shouldDeletePermanently = false)
        {
            var getFileResult = await TryGetFileAsync(path);
            if (getFileResult.IsSuccessful)
            {
                var file = getFileResult.Result;
                await file.DeleteAsync(shouldDeletePermanently
                    ? StorageDeleteOption.PermanentDelete
                    : StorageDeleteOption.Default);

                return true;
            }

            return false;
        }

        public string ResolveInvalidPath(string path, string replaceWith)
        {
            var regexPattern = String.Format("[{0}]", Regex.Escape(new String(Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).ToArray())));
            var regex = new Regex(regexPattern, RegexOptions.Singleline | RegexOptions.CultureInvariant);
            var resolvedPath = regex.Replace(path, replaceWith);
            if (resolvedPath.EndsWith("."))
            {
                if (resolvedPath.Length > 0)
                {
                    return resolvedPath.Substring(0, resolvedPath.Length - 1);
                }

                return replaceWith;
            }

            return resolvedPath;
        }

        public async Task<ulong> GetFreeSpaceAsync(IStorageFolder storageFolder)
        {
            const string freeSpaceFolderProperty = "System.FreeSpace";

            var basicProperties = await storageFolder.GetBasicPropertiesAsync();
            var freeSpace = await basicProperties.RetrievePropertiesAsync(new [] { freeSpaceFolderProperty });
            var bytes = (ulong) freeSpace.Values.Single();

            return bytes;
        }


        private static bool CheckIfPathIsUri(string path)
            => path.StartsWith(MsAppxScheme);
    }
}