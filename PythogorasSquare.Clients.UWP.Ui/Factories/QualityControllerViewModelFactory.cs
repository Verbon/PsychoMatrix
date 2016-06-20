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
            var qualityViewModel = new QualityViewModel(qualityController.Name, qualityController.Power, qualityController.Description);

            return qualityViewModel;
        }
    }
}