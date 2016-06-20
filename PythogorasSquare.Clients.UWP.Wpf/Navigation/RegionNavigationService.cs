using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    [UsedImplicitly]
    public class RegionNavigationService : IRegionNavigationService
    {
        private readonly IRegionManager _regionManager;


        public RegionNavigationService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }


        public bool Navigate(string regionName, string pageToken, object parameter = null)
        {
            return GetRegion(regionName).Navigate(pageToken, parameter);
        }

        public void GoBack(string regionName)
        {
            GetRegion(regionName).GoBack();
        }

        public bool CanGoBack(string regionName)
        {
            return GetRegion(regionName).CanGoBack();
        }

        public void ClearHistory(string regionName)
        {
            GetRegion(regionName).ClearHistory();
        }


        private IRegion GetRegion(string name)
        {
            return _regionManager.Regions[name];
        }
    }
}