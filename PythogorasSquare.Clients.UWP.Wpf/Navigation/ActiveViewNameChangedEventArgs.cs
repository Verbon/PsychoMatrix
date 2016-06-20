using System;

namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    public sealed class ActiveViewNameChangedEventArgs : EventArgs
    {
        public string ActiveViewName { get; }


        public ActiveViewNameChangedEventArgs(string activeViewName)
        {
            ActiveViewName = activeViewName;
        }
    }
}