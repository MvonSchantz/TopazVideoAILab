using System.Collections.Generic;
using System.Linq;

namespace Cybercraft.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T obj)
        {
            return System.Linq.Enumerable.Repeat(obj, 1);
        }

        public static IEnumerable<T> Concat<T>(this T obj, IEnumerable<T> second)
        {
            return System.Linq.Enumerable.Repeat(obj, 1).Concat(second);
        }
    }
}
