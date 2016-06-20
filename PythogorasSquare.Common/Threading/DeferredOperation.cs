using System;
using System.Threading;
using System.Threading.Tasks;

namespace PythogorasSquare.Common.Threading
{
    public class DeferredOperation : IDeferredOperation, IDisposable
    {
        private readonly AutoResetEvent _deferralsCompletedEvent;
        private int _activeDeferrals;
        private bool _isDisposed;


        public DeferredOperation()
        {
            _deferralsCompletedEvent = new AutoResetEvent(true);
        }


        public IOperationDeferral GetDeferral()
        {
            _deferralsCompletedEvent.Reset();
            Interlocked.Increment(ref _activeDeferrals);

            return new DelegateOperationDeferral(OnOperationDeferralCompleted);
        }

        public async Task WaitUntilDeferralsCompletedAsync()
        {
            if (_activeDeferrals > 0)
            {
                await Task.Factory.StartNew(() => _deferralsCompletedEvent.WaitOne());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void OnOperationDeferralCompleted()
        {
            if (Interlocked.Decrement(ref _activeDeferrals) == 0)
            {
                _deferralsCompletedEvent.Set();
            }
        }

        private void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    _deferralsCompletedEvent.Dispose();
                }
                _isDisposed = true;
            }
        }


        ~DeferredOperation()
        {
            Dispose(false);
        }
    }
}