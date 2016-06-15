using System.Collections.Generic;

namespace PythogorasSquare.Common.Extensions
{
    public static class EqualityExtensions
    {
        public static bool EqualsWithComparer<T>(this T obj, T other, IEqualityComparer<T> comparer)
            => comparer.Equals(obj, other);
    }
}