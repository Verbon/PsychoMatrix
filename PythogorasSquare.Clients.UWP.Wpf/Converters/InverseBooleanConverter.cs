namespace PythogorasSquare.Clients.UWP.Wpf.Converters
{
    public class InverseBooleanConverter : BooleanConverter<bool>
    {
        public InverseBooleanConverter()
            : base(false, true)
        {

        }
    }
}