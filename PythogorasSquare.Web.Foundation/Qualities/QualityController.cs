using System.Runtime.Serialization;
using PythogorasSquare.Web.Foundation.Interfaces;

namespace PythogorasSquare.Web.Foundation.Qualities
{
    [DataContract]
    public class QualityController : IQualityController
    {
        [DataMember(Name = "name")]
        public string Name { get; }

        [DataMember(Name = "power")]
        public string Power { get; }

        [DataMember(Name = "description")]
        public string Description { get; }


        public QualityController(string name, string power, string description)
        {
            Name = name;
            Power = power;
            Description = description;
        }
    }
}