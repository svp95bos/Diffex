using Diffex.Abstractions;

namespace Samples;

public class SampleClass1
{
    [DiffexIgnore]
    private int _numberB;
    public int NumberA { get; set; }
}
