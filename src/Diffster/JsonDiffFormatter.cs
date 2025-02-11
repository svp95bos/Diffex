using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diffster;

public class JsonDiffFormatter : IDiffFormatter<List<DateTime>>
{
    public List<DateTime> Format(List<PropertyDifference> differences)
    {
        throw new NotImplementedException();
    }
}
