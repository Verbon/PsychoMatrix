using System;

namespace PythogorasSquare.Common.Threading
{
    internal class DelegateOperationDeferral : IOperationDeferral
    {
        private readonly Action _onOperationCompleted;


        public DelegateOperationDeferral(Action onOperationCompleted)
        {
            _onOperationCompleted = onOperationCompleted;
        }


        public void Complete()
        {
            _onOperationCompleted();
        }
    }
}