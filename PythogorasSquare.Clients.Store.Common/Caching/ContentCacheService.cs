using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using PythogorasSquare.Clients.Store.Common.Storage;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Caching;
using PythogorasSquare.Common.Extensions;
using PythogorasSquare.Common.Serializers;
using PythogorasSquare.Common.Threading;

namespace PythogorasSquare.Clients.Store.Common.Caching
{
    [UsedImplicitly]
    public class ContentCacheService : IContentCacheService
    {
        private const int DelayBeforeSavingChanges = 2500;
        private const int MaxCachedContentAmount = 1000;
        private const string CacheEntriesFileNamePattern = "{0}CacheEntries.json";
        private const string ContentCacheFolderNamePattern = "{0}Cache";

        private readonly IJsonSerializer _jsonSerializer;
        private readonly IStorageManager _storageManager;

        private readonly IDictionary<string, ManualResetEventSlim> _addingContentEvents;
        private readonly IResourceLocker _cacheLocker;

        private bool _isInitialized;
        private ICache<string, string> _contentCache;
        private string _cacheEntiesFileName;
        private string _contentCacheFolderName;


        public ContentCacheService(IJsonSerializer jsonSerializer, IStorageManager storageManager)
        {
            _jsonSerializer = jsonSerializer;
            _storageManager = storageManager;

            _addingContentEvents = new ConcurrentDictionary<string, ManualResetEventSlim>();
            _cacheLocker = new ResourceLocker();
        }


        public async Task<bool> InitializeAsync(string contentTypeIdentifier)
        {
            if (_isInitialized)
            {
                return false;
            }

            _cacheEntiesFileName = String.Format(CacheEntriesFileNamePattern, contentTypeIdentifier);
            _contentCacheFolderName = String.Format(ContentCacheFolderNamePattern, contentTypeIdentifier);
            var cacheEntriesFilePath = GetCacheEntriesFilePath(_cacheEntiesFileName);
            var cacheEntries = await LoadCacheEntriesAsync(cacheEntriesFilePath);
            _contentCache = new Cache<string, string>(cacheEntries, MaxCachedContentAmount);
            _contentCache.EntryRemoved += ContentCacheOnEntryRemoved;

            return _isInitialized = true;
        }

        public async Task<OperationResult<IReadOnlyCollection<string>>> TryGetCachedContentPathesAsync()
        {
            var cacheContenPathes = new List<string>(_contentCache.Entries.Count);
            foreach (var contentKey in _contentCache.Entries.OrderByDescending(e => e.LastAccessTime).Select(e => e.Key))
            {
                var cachedContentPathResult = await TryGetCachedContentPathAsync(contentKey);
                if (cachedContentPathResult.IsSuccessful)
                {
                    cacheContenPathes.Add(cachedContentPathResult.Result);
                }
            }

            return cacheContenPathes.Count > 0 ? cacheContenPathes : OperationResult<IReadOnlyCollection<string>>.CreateUnsuccessful();
        }

        public async Task<OperationResult<string>> TryGetCachedContentPathAsync(string contentKey)
        {
            string cachedContentPath;
            if (_contentCache.TryGetValue(contentKey, out cachedContentPath))
            {
                var getFileResult = await _storageManager.TryGetFileAsync(cachedContentPath);
                if (getFileResult.IsSuccessful)
                {
                    return cachedContentPath;
                }
            }

            return OperationResult<string>.CreateUnsuccessful();
        }

        public async Task<OperationResult<string>> AddContentAsync(string contentKey, Stream contentStream)
        {
            ManualResetEventSlim manualResetEventSlim;
            if (_addingContentEvents.TryGetValue(contentKey, out manualResetEventSlim))
            {
                manualResetEventSlim.Wait();
            }
            using (var addingContentEvent = new ManualResetEventSlim(false))
            {
                try
                {
                    _addingContentEvents.Add(contentKey, addingContentEvent);
                    var contentCacheFile = await CreateCachedContentFile(contentKey).ConfigureAwait(false);
                    using (var fileStream = await contentCacheFile.OpenStreamForWriteAsync())
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                    _contentCache.AddOrUpdate(contentKey, contentCacheFile.Path);
                    SaveCacheEntriesAsync();

                    return contentCacheFile.Path;
                }
                catch (UnauthorizedAccessException)
                {
                    return OperationResult<string>.CreateUnsuccessful();
                }
                finally
                {
                    _addingContentEvents.Remove(contentKey);
                    addingContentEvent.Set();
                }
            }
        }

        public async Task DeleteContentAsync(Predicate<string> contentCacheNameFilter)
        {
            var contentCachesToDelete = _contentCache.Entries.Where(c => contentCacheNameFilter(c.Key)).ToList();
            contentCachesToDelete.ForEach(c => _contentCache.Remove(c.Key));
            await SaveCacheEntriesAsync();
        }


        private async Task<IEnumerable<CacheEntry<string, string>>> LoadCacheEntriesAsync(string cacheEntriesFilePath)
        {
            var getFileResult = await _storageManager.TryGetFileAsync(cacheEntriesFilePath).ConfigureAwait(false);
            if (!getFileResult.IsSuccessful)
            {
                return Enumerable.Empty<CacheEntry<string, string>>();
            }

            var cacheEntriesFile = getFileResult.Result;
            var cacheEntriesJson = await FileIO.ReadTextAsync(cacheEntriesFile);
            if (String.IsNullOrEmpty(cacheEntriesJson))
            {
                return Enumerable.Empty<CacheEntry<string, string>>();
            }

            var cacheEntries = _jsonSerializer.Deserialize<IEnumerable<CacheEntry<string, string>>>(cacheEntriesJson);

            return cacheEntries;
        }

        private async Task SaveCacheEntriesAsync()
        {
            var cacheVersion = _contentCache.Version;
            await Task.Delay(DelayBeforeSavingChanges);
            if (cacheVersion != _contentCache.Version)
            {
                return;
            }

            bool isAccessed;
            using (_cacheLocker.TryGetAccess(out isAccessed))
            {
                if (!isAccessed)
                {
                    return;
                }

                try
                {
                    var cacheEntriesFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(_cacheEntiesFileName, CreationCollisionOption.ReplaceExisting);
                    var cacheEntriesJson = _jsonSerializer.Serialize(_contentCache.Entries);
                    var cacheEntriesInputBuffer = Encoding.UTF8.GetBytes(cacheEntriesJson).AsBuffer();
                    using (var transactedWriteOperation = await cacheEntriesFile.OpenTransactedWriteAsync())
                    {
                        await transactedWriteOperation.Stream.WriteAsync(cacheEntriesInputBuffer);
                        await transactedWriteOperation.CommitAsync();
                    }
                }
                catch(Exception ex)
                {
                    if (ex.IsCatchableExceptionType())
                    {
                        return;
                    }

                    throw;
                }
            }
        }

        private async void ContentCacheOnEntryRemoved(object sender, EntryRemovedEventArgs<string, string> e)
        {
            var contentFilepath = e.CacheEntry.Value;
            _addingContentEvents.Remove(e.CacheEntry.Key);
            await _storageManager.DeleteFileAsync(contentFilepath, true).ConfigureAwait(false);
        }

        private async Task<IStorageFile> CreateCachedContentFile(string contentKey)
        {
            var contentCacheFolder = await GetContentCacheFolderAsync();
            var uniqueContentFileName = String.Format("{0}_{1}", Guid.NewGuid(), contentKey.GetHashCode());
            var cachedContentFile = await contentCacheFolder.CreateFileAsync(uniqueContentFileName, CreationCollisionOption.ReplaceExisting);

            return cachedContentFile;
        }

        private async Task<IStorageFolder> GetContentCacheFolderAsync()
            => await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(_contentCacheFolderName, CreationCollisionOption.OpenIfExists);

        private static string GetCacheEntriesFilePath(string cacheEntriesFileName)
            => Path.Combine(ApplicationData.Current.TemporaryFolder.Path, cacheEntriesFileName);
    }
}