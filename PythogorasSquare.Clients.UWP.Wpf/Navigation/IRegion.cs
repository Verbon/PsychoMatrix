using System;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    public interface IRegion : INavigationService
    {
        string Name { get; }

        string ActiveViewName { get; }


        event EventHandler<ActiveViewNameChangedEventArgs> ActiveViewNameChanged;
    }
}