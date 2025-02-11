using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster;

public class DefaultDiffFormatter : IDiffFormatter
{
    public string Format(PropertyDifference difference)
    {
        return $"{difference.PropertyName}: '{difference.FirstValue}' vs '{difference.SecondValue}'";
    }
}
