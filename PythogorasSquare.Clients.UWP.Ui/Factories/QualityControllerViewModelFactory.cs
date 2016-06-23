using System;
using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.Ui.Interfaces;
using PythogorasSquare.Clients.UWP.Ui.ViewModels.Qualities;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.UWP.Ui.Factories
{
    [UsedImplicitly]
    public class QualityControllerViewModelFactory : IControllerViewModelFactory<IQualityController, QualityViewModel>
    {
        public QualityViewModel CreateFrom(IQualityController qualityController)
        {
            var name = qualityController.Name;
            var power = String.IsNullOrWhiteSpace(qualityController.Power) ? Resources.Resources.No : qualityController.Power;
            var description = qualityController.Description;
            var qualityViewModel = new QualityViewModel(name, power, description);

            return qualityViewModel;
        }
    }
}