using System;
using System.Runtime.Serialization;

namespace PythogorasSquare.Common.Caching
{
    [DataContract]
    public class CacheEntry<TKey, TValue>
    {
        [DataMember(Name = "key")]
        public TKey Key { get; set; }

        [DataMember(Name = "value")]
        public TValue Value { get; set; }

        [DataMember(Name = "lastAccessTime")]
        public DateTime LastAccessTime { get; set; }
    }
}