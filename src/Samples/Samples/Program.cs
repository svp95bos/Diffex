// See https://aka.ms/new-console-template for more information

using Samples;
using Diffster;
using System.Text.Json;

var instance1 = new SampleClass1();
var instance2 = new SampleClass1();

instance1.NumberA = 1;
instance2.NumberA = 2;

var defaultDiff = instance1.Diff(instance2);
Console.WriteLine("Default");
Console.WriteLine(defaultDiff);

// First usage sample
var differences = instance1.Diff(instance2, differences =>
    JsonSerializer.Serialize(differences)
);
Console.WriteLine("Differences");
Console.WriteLine(differences);

// Second usage sample
Func<List<PropertyDifference>, string> customFormatter = differences =>
{
    return string.Join("\n", differences.Select(d => $"{d.PropertyName}: '{d.FirstValue}' vs '{d.SecondValue}'"));
};

var customFormattedDifferences = instance1.Diff(instance2, customFormatter);
Console.WriteLine("Custom");
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
Console.WriteLine("Json with total");
Console.WriteLine(jsonFormattedDifferencesWithTotal);

// Fourth usage sample
Func<List<PropertyDifference>, List<(string PropertyName, object FirstValue, object SecondValue)>> tupleFormatter = differences =>
{
    return differences.Select(d => (d.PropertyName, d.FirstValue, d.SecondValue)).ToList();
};

var tupleFormattedDifferences = instance1.Diff(instance2, tupleFormatter);
Console.WriteLine("Tuples");
foreach (var diff in tupleFormattedDifferences)
{
    Console.WriteLine($"Property: {diff.PropertyName}, FirstValue: {diff.FirstValue}, SecondValue: {diff.SecondValue}");
}

// Fifth usage sample
Func<List<PropertyDifference>, List<KeyValuePair<string, (object FirstValue, object SecondValue)>>> keyValuePairFormatter = differences =>
{
    return differences.Select(d => new KeyValuePair<string, (object, object)>(d.PropertyName, (d.FirstValue, d.SecondValue))).ToList();
};

var keyValuePairFormattedDifferences = instance1.Diff(instance2, keyValuePairFormatter);
Console.WriteLine("Key-Value Pairs");
foreach (var kvp in keyValuePairFormattedDifferences)
{
    Console.WriteLine($"Property: {kvp.Key}, FirstValue: {kvp.Value.FirstValue}, SecondValue: {kvp.Value.SecondValue}");
}

Console.ReadLine();
