using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using PythogorasSquare.Clients.UWP.Wpf.Events;
using PythogorasSquare.Clients.UWP.Wpf.Navigation;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Threading;

namespace PythogorasSquare.Clients.UWP.Wpf
{
    [UsedImplicitly]
    public abstract class MvvmApplicationBase : Application
    {
        private bool _isExiting;


        protected IRegionManager RegionManager { get; private set; }

        protected IRegionNavigationService RegionNavigationService { get; private set; }

        protected ISessionStateService SessionStateService { get; private set; }

        protected IEventAggregator EventAggregator { get; private set; }


        protected MvvmApplicationBase()
        {
            EventAggregatorContext.Current = EventAggregator = new EventAggregator();
        }


        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            await InitializeShellAsync(args);
            await OnLaunchApplicationAsync(args);
            Window.Current.Activate();
            await OnLaunchedAsync();
        }

        protected abstract IShell CreateShell();

        protected virtual Task OnInitializeAsync(IActivatedEventArgs args)
            => Task.FromResult<object>(null);

        protected abstract Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args);

        protected virtual Task OnLaunchedAsync()
            => Task.FromResult<object>(null);

        protected abstract Type GetPageType(string pageToken);

        protected virtual object Resolve(Type type)
            => Activator.CreateInstance(type);


        private async Task InitializeShellAsync(IActivatedEventArgs args)
        {
            var shell = Window.Current.Content as IShell;
            if (shell == null)
            {
                SessionStateService = new SessionStateService();
                RegionManager = new RegionManager(GetPageType, SessionStateService);
                RegionNavigationService = new RegionNavigationService(RegionManager);
                RegionManagerContext.Current = RegionManager;
                shell = CreateShell();
                VisualStateAwarePage.GetSessionStateForFrame = frame => SessionStateService.GetSessionStateForFrame(frame);

                var isHardwareButtonsApiPresent = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
                if (isHardwareButtonsApiPresent)
                {
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
                }

                ViewModelLocationProvider.SetDefaultViewModelFactory(Resolve);
                await OnInitializeAsync(args);
                Window.Current.Content = (UIElement)shell;
            }
        }

        private async void HardwareButtonsOnBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (_isExiting)
            {
                return;
            }
            var backPressedEventArgs = new BackPressedEventArgs();
            EventAggregator.GetEvent<BackPressedEvent>().Publish(backPressedEventArgs);
            if (backPressedEventArgs.Handled)
            {
                return;
            }
            _isExiting = true;
            var deferredOperation = new DeferredOperation();
            EventAggregator.GetEvent<ApplicationExitingEvent>().Publish(deferredOperation);
            await deferredOperation.WaitUntilDeferralsCompletedAsync();
            Exit();
        }
    }
}