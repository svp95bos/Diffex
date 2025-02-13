namespace Diffex;

/// <summary>
/// Represents a difference between two properties.
/// </summary>
public class PropertyDifference
{
    /// <summary>
    /// Gets or sets the name of the property.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the value of the property in the first object.
    /// </summary>
    public object FirstValue { get; set; }

    /// <summary>
    /// Gets or sets the value of the property in the second object.
    /// </summary>
    public object SecondValue { get; set; }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that contains the property name, first value, and second value.</returns>
    public override string ToString()
    {
        var propName = string.IsNullOrEmpty(PropertyName) ? "null" : $"{PropertyName}";
        return $"{propName};{FirstValue};{SecondValue}";
    }
}


