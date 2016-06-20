using Windows.UI.Xaml;
using PythogorasSquare.Clients.UWP.Wpf;
using PythogorasSquare.Clients.UWP.Wpf.Interfaces;

namespace PythogorasSquare.Clients.UWP
{
    public sealed partial class Shell : IShell, IOverlayable
    {
        public Shell()
        {
            InitializeComponent();
        }


        void IOverlayable.ShowOverlay()
        {
            Overlay.Visibility = Visibility.Visible;
        }

        void IOverlayable.HideOverlay()
        {
            Overlay.Visibility = Visibility.Collapsed;
        }
    }
}