using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffster;

public interface IDiffFormatter<TOutput>
{
    TOutput Format(List<PropertyDifference> differences);
}
