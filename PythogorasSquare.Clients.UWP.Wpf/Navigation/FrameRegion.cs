using System;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using PythogorasSquare.Common.Extensions;

namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    public class FrameRegion : IRegion
    {
        private readonly INavigationService _navigationService;
        private readonly FrameFacadeAdapter _frame;

        private string _activeViewName;


        public string Name { get; }

        public string ActiveViewName
        {
            get
            {
                return _activeViewName;
            }
            private set
            {
                if (_activeViewName != value)
                {
                    _activeViewName = value;
                    ActiveViewNameChanged.RaiseEvent(this, new ActiveViewNameChangedEventArgs(_activeViewName));
                }
            }
        }


        public event EventHandler<ActiveViewNameChangedEventArgs> ActiveViewNameChanged;


        public FrameRegion(string name, Frame frame, Func<string, Type> navigationResolver, ISessionStateService sessionStateService)
        {
            Name = name;

            _frame = new FrameFacadeAdapter(frame);
            _frame.Navigated += FrameOnNavigated;
            sessionStateService.RegisterFrame(_frame, name);
            _navigationService = new FrameNavigationService(_frame, navigationResolver, sessionStateService);
        }


        public bool Navigate(string pageToken, object parameter)
        {
            return _navigationService.Navigate(pageToken, parameter);
        }

        public void GoBack()
        {
            _navigationService.GoBack();
        }

        public bool CanGoBack()
        {
            return _navigationService.CanGoBack();
        }

        public void ClearHistory()
        {
            _navigationService.ClearHistory();
        }

        public void RestoreSavedNavigation()
        {
            _navigationService.RestoreSavedNavigation();
        }

        public void Suspending()
        {
            _navigationService.Suspending();
        }


        private void FrameOnNavigated(object sender, MvvmNavigatedEventArgs e)
        {
            ActiveViewName = _frame.Content.GetType().Name;
        }
    }
}