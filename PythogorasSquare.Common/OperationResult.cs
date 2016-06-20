using System;

namespace PythogorasSquare.Common
{
    public class OperationResult<T>
    {
        private readonly T _result;


        public bool IsSuccessful { get; }

        public T Result
        {
            get
            {
                if (!IsSuccessful)
                {
                    throw new InvalidOperationException();
                }

                return _result;
            }
        }


        private OperationResult(bool isSuccessful, T result)
        {
            IsSuccessful = isSuccessful;
            _result = result;
        }


        public static OperationResult<T> CreateUnsuccessful()
            => new OperationResult<T>(false, default(T));

        public static implicit operator OperationResult<T>(T result)
            => CreateSuccessful(result);


        private static OperationResult<T> CreateSuccessful(T result)
            => new OperationResult<T>(true, result);
    }
}