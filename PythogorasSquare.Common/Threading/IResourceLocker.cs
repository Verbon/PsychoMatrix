using System;

namespace PythogorasSquare.Common.Threading
{
    public interface IResourceLocker
    {
        bool IsLocked { get; }


        IDisposable TryGetAccess(out bool isAccessed);

        bool TryGetAccess();

        bool ReleaseAccess();
    }
}