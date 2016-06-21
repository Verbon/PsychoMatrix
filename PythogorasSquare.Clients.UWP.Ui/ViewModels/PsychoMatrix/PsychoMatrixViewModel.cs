using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.Ui.Interfaces;
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

        private DateTime _birthDate;
        private readonly ObservableCollection<QualityViewModel> _qualities;
        private QualityViewModel _selectedQuality;


        public DateTime BirthDate
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
            IControllerViewModelProvider<IQualityController, QualityViewModel> qualityViewModelProvider)
        {
            _psychoMatrixService = psychoMatrixService;
            _qualityViewModelProvider = qualityViewModelProvider;

            _qualities = new ObservableCollection<QualityViewModel>();
            RefreshPsychoMatrixFor(new DateTime(1996, 2, 26));
        }


        private async void RefreshPsychoMatrixFor(DateTime birthDate)
        {
            var qualityControllers = await _psychoMatrixService.GetPsychoMatrixForAsync(birthDate);
            var qualityViewModels = qualityControllers.Select(_qualityViewModelProvider.GetViewModelFor).ToList();

            _qualities.RefillWith(qualityViewModels);
        }
    }
}