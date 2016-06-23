using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

            BirthDate = DateTimeOffset.Now;
        }


        private async void RefreshPsychoMatrixFor(DateTimeOffset birthDate)
        {
            var qualityControllers = await _psychoMatrixService.GetPsychoMatrixForAsync(birthDate.DateTime);
            var qualityViewModels = qualityControllers.Select(_qualityViewModelProvider.GetViewModelFor).ToList();

            _qualities.RefillWith(qualityViewModels);
            SelectedQuality = _qualities.First();
        }
    }
}