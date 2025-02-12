using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster;

public static class DifferExtensions
{
    public static TOutput Diff<T, TOutput>(this T first, T second, Func<List<PropertyDifference>, TOutput> formatter = null)
    {
        if (formatter == null)
        {
            return new Diffster<T, TOutput>().Diff(first, second);
        }
        return new Diffster<T, TOutput>(formatter).Diff(first, second);
    }

    public static string Diff<T>(this T first, T second)
    {
        return new Diffster<T, string>().Diff(first, second);
    }
}
