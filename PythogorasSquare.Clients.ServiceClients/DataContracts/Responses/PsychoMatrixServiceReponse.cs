using System.Collections.Generic;
using System.Runtime.Serialization;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.ServiceClients.DataContracts.Responses
{
    [DataContract]
    public class PsychoMatrixServiceReponse : ServiceResponse
    {
        [DataMember(Name = "qualities")]
        public ICollection<ServiceQuality> Qualities { get; [UsedImplicitly] set; }
    }
}