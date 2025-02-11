using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster;

public class JsonDiffFormatter : IDiffFormatter
{
    public string Format(PropertyDifference difference)
    {
        return $"{{ \"property\": \"{difference.PropertyName}\", \"old\": \"{difference.FirstValue}\", \"new\": \"{difference.SecondValue}\" }}";
    }
}
