using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster
{
    public static class DifferExtensions
    {
        public static List<string> Diff<T>(this T first, T second, IDiffFormatter formatter = null)
        {
            return new Diffster<T>(formatter).Diff(first, second);
        }
    }
}
