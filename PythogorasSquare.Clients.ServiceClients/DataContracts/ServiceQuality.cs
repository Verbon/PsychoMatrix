using System.Runtime.Serialization;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.ServiceClients.DataContracts
{
    [DataContract]
    public class ServiceQuality
    {
        [DataMember(Name = "name")]
        public string Name { get; [UsedImplicitly] set; }

        [DataMember(Name = "power")]
        public string Power { get; [UsedImplicitly] set; }

        [DataMember(Name = "description")]
        public string Description { get; [UsedImplicitly] set; }
    }
}