using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster
{
    public static class DifferExtensions
    {
        public static List<PropertyDifference> Diff<T>(this T first, T second)
        {
            return new Diffster<T>().Diff(first, second);
        }
    }
}
