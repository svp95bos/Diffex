using System;
using System.Collections.Generic;
using System.Text;

namespace Diffex.Abstractions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DiffexIgnoreAttribute : Attribute
{
}
