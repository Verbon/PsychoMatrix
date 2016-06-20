using System;

namespace PythogorasSquare.Common
{
    public class AnonymousDisposable : IDisposable
    {
        private readonly Action _dipose;


        public AnonymousDisposable(Action dipose)
        {
            _dipose = dipose;
        }


        public void Dispose()
        {
            _dipose();
        }
    }
}