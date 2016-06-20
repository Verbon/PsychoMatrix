using System;
using System.Threading;
using System.Threading.Tasks;

namespace PythogorasSquare.Common.Threading
{
    public class AsyncResourceLocker : IAsyncResourceLocker
    {
        private readonly SemaphoreSlim _semaphore;


        public AsyncResourceLocker()
        {
            _semaphore = new SemaphoreSlim(1, 1);
        }


        public async Task<IDisposable> TryGetAccessAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            return new AnonymousDisposable(() => _semaphore.Release());
        }
    }
}