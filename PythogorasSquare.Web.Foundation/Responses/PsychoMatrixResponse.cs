using System.Collections.Generic;
using System.Runtime.Serialization;
using PythogorasSquare.Web.Foundation.Interfaces;

namespace PythogorasSquare.Web.Foundation.Responses
{
    [DataContract]
    public sealed class PsychoMatrixResponse : Response
    {
        [DataMember(Name = "qualities")]
        public IReadOnlyCollection<IQualityController> Qualities { get; }


        public PsychoMatrixResponse(string status, IReadOnlyCollection<IQualityController> qualities)
            : base(status)
        {
            Qualities = qualities;
        }
    }
}