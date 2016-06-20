using System.Collections.Generic;

namespace PythogorasSquare.Clients.UWP.Wpf.Navigation
{
    public interface IRegionManager
    {
        IReadOnlyDictionary<string, IRegion> Regions { get; }


        IRegion CreateRegion(string name);
    }
}