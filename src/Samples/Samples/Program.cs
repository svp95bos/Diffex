// See https://aka.ms/new-console-template for more information

using Samples;
using Diffster;
using System.Text.Json;

var instance1 = new SampleClass1();
var instance2 = new SampleClass1();

instance1.NumberA = 1;
instance2.NumberA = 2;

// First usage sample
var differences = instance1.Diff(instance2, differences =>
    JsonSerializer.Serialize(differences)
);
Console.WriteLine(differences);

// Second usage sample
Func<List<PropertyDifference>, string> customFormatter = differences =>
{
    return string.Join("\n", differences.Select(d => $"{d.PropertyName}: '{d.FirstValue}' vs '{d.SecondValue}'"));
};

var customFormattedDifferences = instance1.Diff(instance2, customFormatter);
Console.WriteLine(customFormattedDifferences);

// Third usage sample
Func<List<PropertyDifference>, string> jsonFormatterWithTotal = differences =>
{
    var result = new
    {
        TotalDiff = differences.Count,
        Differences = differences
    };
    return JsonSerializer.Serialize(result);
};

var jsonFormattedDifferencesWithTotal = instance1.Diff(instance2, jsonFormatterWithTotal);
Console.WriteLine(jsonFormattedDifferencesWithTotal);

Console.ReadLine();
