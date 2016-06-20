using System;

namespace PythogorasSquare.Clients.UWP.Wpf.ViewViewModelTypeResolver
{
    public interface IViewViewModelTypeResolver
    {
        Type GetViewType(string viewTypeName);

        Type GetViewModelType(Type viewType);
    }
}