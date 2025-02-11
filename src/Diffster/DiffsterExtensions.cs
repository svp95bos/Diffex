using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster;

public static class DifferExtensions
{
    public static TOutput Diff<T, TOutput>(this T first, T second, IDiffFormatter<TOutput> formatter = null)
    {
        return new Diffster<T, TOutput>(formatter).Diff(first, second);
    }
}
