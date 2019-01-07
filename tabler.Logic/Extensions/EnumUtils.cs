using System;
using System.Collections.Generic;

namespace tabler.Logic.Extensions
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
