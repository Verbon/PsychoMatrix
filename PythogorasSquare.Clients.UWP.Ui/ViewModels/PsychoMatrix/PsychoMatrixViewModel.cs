using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.Ui.Interfaces;
using PythogorasSquare.Clients.UWP.Ui.ViewModels.Navigation;
using PythogorasSquare.Clients.UWP.Ui.ViewModels.Qualities;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Extensions;

namespace PythogorasSquare.Clients.UWP.Ui.ViewModels.PsychoMatrix
{
    [UsedImplicitly]
    public class PsychoMatrixViewModel : ViewModel
    {
        private readonly IPsychoMatrixService _psychoMatrixService;
        private readonly IControllerViewModelProvider<IQualityController, QualityViewModel> _qualityViewModelProvider;

        private DateTimeOffset _birthDate;
        private readonly ObservableCollection<QualityViewModel> _qualities;
        private QualityViewModel _selectedQuality;
        private bool _isLoading;


        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            private set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        public NavigationPanelViewModel NavigationPanelViewModel { get; }

        public DateTimeOffset BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                if (SetProperty(ref _birthDate, value))
                {
                    RefreshPsychoMatrixFor(_birthDate);
                    if (NavigationPanelViewModel.IsPanelOpen)
                    {
                        NavigationPanelViewModel.TogglePanelCommand.TryExecute();
                    }
                }
            }
        }

        public IEnumerable<QualityViewModel> Qualities => _qualities;

        public QualityViewModel SelectedQuality
        {
            get
            {
                return _selectedQuality;
            }
            set
            {
                SetProperty(ref _selectedQuality, value);
            }
        }


        public PsychoMatrixViewModel(
            IPsychoMatrixService psychoMatrixService,
            IControllerViewModelProvider<IQualityController, QualityViewModel> qualityViewModelProvider,
            NavigationPanelViewModel navigationPanelViewModel)
        {
            _psychoMatrixService = psychoMatrixService;
            _qualityViewModelProvider = qualityViewModelProvider;
            NavigationPanelViewModel = navigationPanelViewModel;

            _qualities = new ObservableCollection<QualityViewModel>();
        }


        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            IsLoading = true;
            var lastSeenPsychoMatrix = await _psychoMatrixService.GetLastSeenPsychoMatrixOrDefaultAsync();
            _birthDate = lastSeenPsychoMatrix.Item1;
            OnPropertyChanged(() => BirthDate);

            var qualityControllers = lastSeenPsychoMatrix.Item2;
            var qualityViewModels = qualityControllers.Select(_qualityViewModelProvider.GetViewModelFor).ToList();
            _qualities.RefillWith(qualityViewModels);
            SelectedQuality = _qualities.First();
            IsLoading = false;
        }


        private async void RefreshPsychoMatrixFor(DateTimeOffset birthDate)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            var qualityControllersResult = await _psychoMatrixService.GetPsychoMatrixForAsync(birthDate.DateTime);
            if (qualityControllersResult.IsSuccessful)
            {
                var qualityControllers = qualityControllersResult.Result;
                var qualityViewModels = qualityControllers.Select(_qualityViewModelProvider.GetViewModelFor).ToList();

                _qualities.RefillWith(qualityViewModels);
                SelectedQuality = _qualities.First();
            }
            IsLoading = false;
        }
    }
}