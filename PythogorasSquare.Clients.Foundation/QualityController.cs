using PythogorasSquare.Clients.Foundation.Interfaces;

namespace PythogorasSquare.Clients.Foundation
{
    internal class QualityController : IQualityController
    {
        public string Name { get; }

        public string Power { get; }

        public string Description { get; }


        public QualityController(string name, string power, string description)
        {
            Name = name;
            Power = power;
            Description = description;
        }
    }
}