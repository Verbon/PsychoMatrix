using System.Runtime.Serialization;

namespace PythogorasSquare.Web.Foundation.Responses
{
    [DataContract]
    public abstract class Response
    {
        [DataMember(Name = "status")]
        public string Status { get; }


        protected Response(string status)
        {
            Status = status;
        }
    }
}