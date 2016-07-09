using System;
using System.Collections.Generic;
using System.IO;

namespace PythogorasSquare.Common.Caching
{
    public interface ICache<TKey, TValue>
    {
        int Version { get; }

        IReadOnlyCollection<CacheEntry<TKey, TValue>> Entries { get; }


        event EventHandler<EntryRemovedEventArgs<TKey, TValue>> EntryRemoved;


        bool TryGetValue(TKey key, out TValue value);

        void AddOrUpdate(TKey key, TValue value);

        void Remove(TKey key);
    }
}