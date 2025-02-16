using System;

namespace Diffex.Abstractions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DiffexIgnoreAttribute : Attribute
{
}
