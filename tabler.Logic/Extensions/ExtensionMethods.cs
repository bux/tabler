
using System;

namespace tabler.Logic.Extensions
{
    public static class ExtensionMethods
    {
        public static bool ContainsEx(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
