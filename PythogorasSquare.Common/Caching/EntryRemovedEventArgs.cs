using System;

namespace PythogorasSquare.Common.Caching
{
    public sealed class EntryRemovedEventArgs<TKey, TValue> : EventArgs
    {
        public CacheEntry<TKey, TValue> CacheEntry { get; private set; }


        public EntryRemovedEventArgs(CacheEntry<TKey, TValue> cacheEntry)
        {
            CacheEntry = cacheEntry;
        }
    }
}