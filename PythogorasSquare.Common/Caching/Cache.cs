using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PythogorasSquare.Common.Extensions;

namespace PythogorasSquare.Common.Caching
{
    public class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly int _maxSize;
        private readonly IDictionary<TKey, CacheEntry<TKey, TValue>> _cacheEntries;


        private bool IsSizeLimitReached => _cacheEntries.Count >= _maxSize;


        public int Version { get; private set; }

        public IReadOnlyCollection<CacheEntry<TKey, TValue>> Entries => _cacheEntries.Values.ToList();


        public event EventHandler<EntryRemovedEventArgs<TKey, TValue>> EntryRemoved;


        public Cache(int maxSize)
            : this(new List<CacheEntry<TKey, TValue>>(0), maxSize)
        {

        }

        public Cache(IEnumerable<CacheEntry<TKey, TValue>> initialCacheEntries, int maxSize)
        {
            _maxSize = maxSize;
            _cacheEntries = new ConcurrentDictionary<TKey, CacheEntry<TKey, TValue>>(initialCacheEntries.Take(maxSize).ToDictionary(entry => entry.Key));
        }


        public bool TryGetValue(TKey key, out TValue value)
        {
            CacheEntry<TKey, TValue> cacheEntry;
            if (_cacheEntries.TryGetValue(key, out cacheEntry))
            {
                cacheEntry.LastAccessTime = DateTime.UtcNow;
                value = cacheEntry.Value;

                return true;
            }

            value = default(TValue);

            return false;
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            var cacheEntry = new CacheEntry<TKey, TValue> { Key = key, Value = value, LastAccessTime = DateTime.UtcNow };
            if (!_cacheEntries.ContainsKey(key) && IsSizeLimitReached)
            {
                RemoveLeastRecentlyUsedEntry();
            }
            _cacheEntries[key] = cacheEntry;
            Version++;
        }

        public void Remove(TKey key)
        {
            CacheEntry<TKey, TValue> cacheEntry;
            if (_cacheEntries.TryGetValue(key, out cacheEntry))
            {
                _cacheEntries.Remove(key);
                Version++;
                OnEntryRemoved(cacheEntry);
            }
        }


        private void RemoveLeastRecentlyUsedEntry()
        {
            var leastRecentlyUsedEntry = _cacheEntries.Values.OrderBy(entry => entry.LastAccessTime).First();
            _cacheEntries.Remove(leastRecentlyUsedEntry.Key);
            Version++;
            OnEntryRemoved(leastRecentlyUsedEntry);
        }

        private void OnEntryRemoved(CacheEntry<TKey, TValue> removedCacheEntry)
            => EntryRemoved.RaiseEvent(this, new EntryRemovedEventArgs<TKey, TValue>(removedCacheEntry));
    }
}