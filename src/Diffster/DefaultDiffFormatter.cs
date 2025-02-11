using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster;

public class DefaultDiffFormatter : IDiffFormatter<string>
{
    public string Format(List<PropertyDifference> differences)
    {
        return string.Join("\n", differences.Select(d => $"{d.PropertyName}: '{d.FirstValue}' vs '{d.SecondValue}'"));
    }
}
