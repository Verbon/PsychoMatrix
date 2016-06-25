using System;
using System.Threading.Tasks;

namespace PythogorasSquare.Common.Configuration
{
    public interface IAppConfigServiceInitializer
    {
        Task InitializeAsync(Uri appConfigUri);
    }
}