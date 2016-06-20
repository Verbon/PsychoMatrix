using System.Runtime.Serialization;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.ServiceClients.DataContracts.Responses
{
    [DataContract]
    public abstract class ServiceResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; [UsedImplicitly] set; }

        public bool IsSuccessful => Status == ServiceResponses.Success;
    }
}