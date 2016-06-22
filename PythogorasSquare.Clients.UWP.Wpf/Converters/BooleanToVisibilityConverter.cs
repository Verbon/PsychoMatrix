using Windows.UI.Xaml;

namespace PythogorasSquare.Clients.UWP.Wpf.Converters
{
    public class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter()
            : base(Visibility.Visible, Visibility.Collapsed)
        {

        }
    }
}