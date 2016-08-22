using System;
using PythogorasSquare.Web.Resources.MultilingualResources;

namespace PythogorasSquare.WebApp.ViewModels.Qualities
{
    public class QualityViewModel
    {
        public string Name { get; }

        public string Power { get; }

        public string Description { get; }


        public QualityViewModel(string name, string power, string description)
        {
            Name = name;
            Power = String.IsNullOrWhiteSpace(power) ? PsResources.No : power;
            Description = description;
        }
    }
}