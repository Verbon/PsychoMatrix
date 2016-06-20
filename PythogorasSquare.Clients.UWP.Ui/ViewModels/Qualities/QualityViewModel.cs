using Microsoft.Practices.Prism.Mvvm;

namespace PythogorasSquare.Clients.UWP.Ui.ViewModels.Qualities
{
    public class QualityViewModel : BindableBase
    {
        public string Name { get; }

        public string Power { get; }

        public string Description { get; }


        public QualityViewModel(string name, string power, string description)
        {
            Name = name;
            Power = power;
            Description = description;
        }
    }
}