using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    public class RegionManager : IRegionManager
    {
        private readonly Func<string, Type> _navigationResolver;
        private readonly ISessionStateService _sessionStateService;

        private readonly IDictionary<string, Frame> _regionFrames;
        private readonly Dictionary<string, IRegion> _regions;


        public static readonly DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager), new PropertyMetadata(default(string), OnRegionNamePropertyValueChanged));


        public static void SetRegionName(DependencyObject contentControl, string regionName)
        {
            contentControl.SetValue(RegionNameProperty, regionName);
        }

        public static string GetRegionName(DependencyObject contentControl)
        {
            return (string)contentControl.GetValue(RegionNameProperty);
        }


        public IReadOnlyDictionary<string, IRegion> Regions => _regions;


        private static event RegionCreationRequestedHandler RegionCreationRequested;


        public RegionManager(Func<string, Type> navigationResolver, ISessionStateService sessionStateService)
        {
            _navigationResolver = navigationResolver;
            _sessionStateService = sessionStateService;

            _regionFrames = new Dictionary<string, Frame>();
            _regions = new Dictionary<string, IRegion>();

            RegionCreationRequested += OnRegionCreationRequested;
        }


        public IRegion CreateRegion(string name)
        {
            IRegion region;
            if (_regions.TryGetValue(name, out region))
            {
                return region;
            }

            var frame = new Frame();
            _regionFrames.Add(name, frame);
            region = new FrameRegion(name, frame, _navigationResolver, _sessionStateService);
            _regions.Add(name, region);

            return region;
        }


        private void OnRegionCreationRequested(ContentControl contentControl, string regionName)
        {
            if (!_regions.ContainsKey(regionName))
            {
                CreateRegion(regionName);
            }
            var regionFrame = _regionFrames[regionName];
            contentControl.Content = regionFrame;
        }

        private static void OnRegionNamePropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var contentControl = (ContentControl)d;
            var regionName = (string)e.NewValue;
            var regionCreationRequested = RegionCreationRequested;

            regionCreationRequested?.Invoke(contentControl, regionName);
        }



        private delegate void RegionCreationRequestedHandler(ContentControl contentControl, string regionName);
    }
}