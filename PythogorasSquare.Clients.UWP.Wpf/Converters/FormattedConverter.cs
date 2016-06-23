using System;
using Windows.UI.Xaml.Data;

namespace PythogorasSquare.Clients.UWP.Wpf.Converters
{
    public class FormattedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => String.Format("{0:" + parameter + "}", value);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => new NotImplementedException();
    }
}