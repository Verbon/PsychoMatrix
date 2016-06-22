using System;
using Windows.UI.Xaml.Data;

namespace PythogorasSquare.Clients.UWP.Wpf.Converters
{
    public abstract class BooleanConverter<T> : IValueConverter
    {
        private readonly T _trueValue;
        private readonly T _falseValue;


        protected BooleanConverter(T trueValue, T falseValue)
        {
            _trueValue = trueValue;
            _falseValue = falseValue;
        }


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToBoolean(value) ? _trueValue : _falseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is T && ((T) value).Equals(_trueValue);
        }
    }
}