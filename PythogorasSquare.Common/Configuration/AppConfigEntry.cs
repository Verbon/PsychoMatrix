using System;
using System.Runtime.Serialization;

namespace PythogorasSquare.Common.Configuration
{
    [DataContract]
    public class AppConfigEntry
    {
        [DataMember]
        public bool IsApplicationLevel { get; [UsedImplicitly] set; }

        public AppConfigEntryType SerializeAs => (AppConfigEntryType) Enum.Parse(typeof(AppConfigEntryType), SerializeAsString);

        [DataMember(Name = "SerializeAs")]
        public string SerializeAsString { get; [UsedImplicitly] set; }

        [DataMember]
        public string Name { get; [UsedImplicitly] set; }

        [DataMember]
        public string Value { get; [UsedImplicitly] set; }
    }
}