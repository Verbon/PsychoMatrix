using System;
using System.Threading;

namespace PythogorasSquare.Common.Threading
{
    public class ResourceLocker : IResourceLocker
    {
        private int _isLocked;


        public bool IsLocked => _isLocked == 1;


        public IDisposable TryGetAccess(out bool isAccessed)
        {
            var isAcessedLocal = isAccessed = Interlocked.Exchange(ref _isLocked, 1) == 0;

            return new AnonymousDisposable(() =>
            {
                if (isAcessedLocal)
                {
                    ReleaseAccess();
                }
            });
        }

        public bool TryGetAccess()
            => Interlocked.Exchange(ref _isLocked, 1) == 0;

        public bool ReleaseAccess()
            => Interlocked.Exchange(ref _isLocked, 0) == 1;
    }
}