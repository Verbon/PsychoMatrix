using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PythogorasSquare.Common.Configuration
{
    [DataContract]
    public class AppConfig
    {
        [DataMember]
        public string Version { get; [UsedImplicitly] set; }

        [DataMember]
        public ICollection<AppConfigEntry> Entries { get; [UsedImplicitly] set; }
    }
}