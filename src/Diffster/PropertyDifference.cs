namespace Diffster;

public class PropertyDifference
{
    public string PropertyName { get; set; }
    public object FirstValue { get; set; }
    public object SecondValue { get; set; }

    public override string ToString()
    {
        var propName = string.IsNullOrEmpty(PropertyName) ? "null" : $"{PropertyName}";
        return $"{propName};{FirstValue};{SecondValue}";
    }
}


