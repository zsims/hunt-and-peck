using System;
using System.Collections.Generic;
using System.Linq;

namespace HuntAndPeck.Extensions
{
    public static class IEnumerableTExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var rnd = new Random();
            return source.OrderBy<T, int>((item) => rnd.Next());
        }
    }
}
