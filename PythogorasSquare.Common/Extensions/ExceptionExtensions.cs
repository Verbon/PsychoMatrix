using System;

namespace PythogorasSquare.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool IsCatchableExceptionType(this Exception ex)
            => ex.GetType() != typeof (OutOfMemoryException);
    }
}