using Windows.UI.Xaml;

namespace PythogorasSquare.Clients.UWP.Wpf.Converters
{
    public class InverseBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InverseBooleanToVisibilityConverter()
            : base(Visibility.Collapsed, Visibility.Visible)
        {

        }
    }
}