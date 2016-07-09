using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PythogorasSquare.Common.Caching
{
    public interface IContentCacheService
    {
        Task<bool> InitializeAsync(string contentTypeIdentifier);

        Task<OperationResult<IReadOnlyCollection<string>>> TryGetCachedContentPathesAsync();

        Task<OperationResult<string>> TryGetCachedContentPathAsync(string contentKey);

        Task<OperationResult<string>> AddContentAsync(string contentKey, Stream contentStream);

        Task DeleteContentAsync(Predicate<string> contentCacheNameFilter);
    }
}