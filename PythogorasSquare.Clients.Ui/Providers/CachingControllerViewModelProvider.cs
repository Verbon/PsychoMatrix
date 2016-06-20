using System.Collections.Generic;
using System.Linq;
using PythogorasSquare.Clients.Ui.Interfaces;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.Ui.Providers
{
    [UsedImplicitly]
    public class CachingControllerViewModelProvider<TController, TViewModel> : IControllerViewModelProvider<TController, TViewModel>
    {
        private readonly IControllerViewModelFactory<TController, TViewModel> _controllerViewModelFactory;

        private readonly Dictionary<TController, TViewModel> _viewModels;


        public CachingControllerViewModelProvider(IControllerViewModelFactory<TController, TViewModel> controllerViewModelFactory)
        {
            _controllerViewModelFactory = controllerViewModelFactory;

            _viewModels = new Dictionary<TController, TViewModel>();
        }


        public TViewModel GetViewModelFor(TController controller)
        {
            TViewModel viewModel;
            if (_viewModels.TryGetValue(controller, out viewModel))
            {
                return viewModel;
            }

            viewModel = _controllerViewModelFactory.CreateFrom(controller);
            _viewModels.Add(controller, viewModel);

            return viewModel;
        }

        public TController GetControllerOf(TViewModel viewModel)
            => _viewModels.Single(pair => pair.Value.Equals(viewModel)).Key;
    }
}