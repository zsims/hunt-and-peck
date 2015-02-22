using System;
using System.Linq;
using System.Collections.Generic;

namespace hap.Engine.Extensions
{
    public static class IEnumerableTExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy<T, int>((item) => rnd.Next());
        }
    }
}
