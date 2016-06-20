using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Microsoft.ApplicationInsights;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using PythogorasSquare.Clients.Foundation.Factories;
using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.Foundation.Services;
using PythogorasSquare.Clients.ServiceClients;
using PythogorasSquare.Clients.ServiceClients.DataContracts;
using PythogorasSquare.Clients.ServiceClients.Interfaces;
using PythogorasSquare.Clients.Ui.Interfaces;
using PythogorasSquare.Clients.Ui.Providers;
using PythogorasSquare.Clients.UWP.Ui;
using PythogorasSquare.Clients.UWP.Ui.Factories;
using PythogorasSquare.Clients.UWP.Ui.ViewModels.Qualities;
using PythogorasSquare.Clients.UWP.Wpf;
using PythogorasSquare.Clients.UWP.Wpf.ViewViewModelTypeResolver;
using PythogorasSquare.Common.Serializers;
using PythogorasSquare.Foundation.Interfaces;
using PythogorasSquare.Foundation.Providers;

namespace PythogorasSquare.Clients.UWP
{
    public sealed partial class App
    {
        private readonly UnityContainer _container;
        private readonly IViewViewModelTypeResolver _viewViewModelTypeResolver;


        public App()
        {
            WindowsAppInitializer.InitializeAsync(WindowsCollectors.Metadata | WindowsCollectors.Session);
            InitializeComponent();
            _container = new UnityContainer();
            var uiAssembly = typeof(PsychoMatrixViews).GetTypeInfo().Assembly;
            _viewViewModelTypeResolver = new ViewViewModelTypeResolver(uiAssembly);
        }


        protected override IShell CreateShell()
            => new Shell();

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            _container.RegisterInstance(_viewViewModelTypeResolver);
            _container.RegisterInstance(new Func<Type, object>(Resolve));
            _container.RegisterInstance(RegionNavigationService);

            _container.RegisterType<IJsonSerializer, JsonSerializer>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IPythogorasSquareService, PythogorasSquareService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IEntityControllerFactory<ServiceQuality, IQualityController>, ServiceQualityControllerFactory>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IEqualityComparer<ServiceQuality>, ServiceQualityEqualityComparer>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IEntityControllerProvider<ServiceQuality, IQualityController>, CachingEntityControllerProvider<ServiceQuality, IQualityController>>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IControllerViewModelFactory<IQualityController, QualityViewModel>, QualityControllerViewModelFactory>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IControllerViewModelProvider<IQualityController, QualityViewModel>, CachingControllerViewModelProvider<IQualityController, QualityViewModel>>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IPsychoMatrixService, PsychoMatrixService>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewModelFactory(Resolve);
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(GetViewModelType);

            return base.OnInitializeAsync(args);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            ConfigureTitleBar();
            RegionNavigationService.Navigate(PsychoMatrixRegions.MainRegion, PsychoMatrixViews.PsychoMatrixPage);

            return Task.FromResult<object>(null);
        }

        protected override Type GetPageType(string pageToken)
            => _viewViewModelTypeResolver.GetViewType(pageToken);

        protected override object Resolve(Type type)
            => _container.Resolve(type);


        private Type GetViewModelType(Type pageType)
            => _viewViewModelTypeResolver.GetViewModelType(pageType);

        private static void ConfigureTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var blueColor = Color.FromArgb(255, 52, 152, 219);
            var whiteColor = Color.FromArgb(255, 236, 240, 241);
            var grayColor = Color.FromArgb(255, 149, 165, 166);
            var lighthGrayColor = Color.FromArgb(255, 189, 195, 199);

            titleBar.BackgroundColor = titleBar.InactiveBackgroundColor = blueColor;
            titleBar.ButtonBackgroundColor = titleBar.ButtonInactiveBackgroundColor = blueColor;
            titleBar.ForegroundColor = whiteColor;
            titleBar.ButtonForegroundColor = whiteColor;
            titleBar.ButtonHoverBackgroundColor = grayColor;
            titleBar.ButtonHoverForegroundColor = whiteColor;
            titleBar.ButtonInactiveForegroundColor = titleBar.InactiveForegroundColor = lighthGrayColor;
        }
    }
}