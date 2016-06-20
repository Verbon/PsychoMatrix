namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    public interface IRegionNavigationService
    {
        bool Navigate(string regionName, string pageToken, object parameter = null);

        void GoBack(string regionName);

        bool CanGoBack(string regionName);

        void ClearHistory(string regionName);
    }
}