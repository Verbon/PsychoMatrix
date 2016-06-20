using System;
using System.Threading;
using System.Threading.Tasks;

namespace PythogorasSquare.Common.Threading
{
    public interface IAsyncResourceLocker
    {
        Task<IDisposable> TryGetAccessAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}