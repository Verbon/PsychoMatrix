using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.UWP.Ui.ViewModels.Navigation
{
    [UsedImplicitly]
    public class NavigationPanelViewModel : BindableBase
    {
        private bool _isPanelOpen;


        public bool IsPanelOpen
        {
            get
            {
                return _isPanelOpen;
            }
            set
            {
                SetProperty(ref _isPanelOpen, value);
            }
        }


        public ICommand TogglePanelCommand { get; }


        public NavigationPanelViewModel()
        {
            TogglePanelCommand = new DelegateCommand(TogglePanel);
        }


        private void TogglePanel()
            => IsPanelOpen = !IsPanelOpen;
    }
}