
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diffster;

public class PropertyDifference
{
    public string PropertyName { get; set; }
    public object FirstValue { get; set; }
    public object SecondValue { get; set; }

    public override string ToString()
    {
        return $"{PropertyName}: '{FirstValue}' vs '{SecondValue}'";
    }
}

public class Diffster<T>
{
    private readonly IDiffFormatter _formatter;

    public Diffster(IDiffFormatter formatter = null)
    {
        _formatter = formatter ?? new DefaultDiffFormatter();
    }

    public List<PropertyDifference> Diff(T first, T second, string parentPath = "")
    {
        if (first == null || second == null)
        {
            throw new ArgumentNullException("Both instances must be non-null.");
        }

        List<PropertyDifference> differences = new List<PropertyDifference>();
        Type type = typeof(T);

        if (type.IsEnum)
        {
            // Compare enums using their names instead of raw values
            if (!Equals(first, second))
            {
                differences.Add(new PropertyDifference
                {
                    PropertyName = parentPath,
                    FirstValue = Enum.GetName(type, first),
                    SecondValue = Enum.GetName(type, second)
                });
            }
            return differences;
        }

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            object firstValue = property.GetValue(first);
            object secondValue = property.GetValue(second);
            string propertyPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";

            if (IsEnum(property.PropertyType))
            {
                if (!Equals(firstValue, secondValue))
                {
                    differences.Add(new PropertyDifference
                    {
                        PropertyName = propertyPath,
                        FirstValue = firstValue != null ? Enum.GetName(property.PropertyType, firstValue) : "null",
                        SecondValue = secondValue != null ? Enum.GetName(property.PropertyType, secondValue) : "null"
                    });
                }
            }
            else if (IsCollection(property.PropertyType))
            {
                differences.AddRange(CompareCollections(firstValue as IEnumerable, secondValue as IEnumerable, propertyPath));
            }
            else if (IsComplexType(property.PropertyType) || IsStruct(property.PropertyType))
            {
                if (firstValue == null || secondValue == null)
                {
                    if (!Equals(firstValue, secondValue))
                    {
                        differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
                    }
                }
                else
                {
                    var nestedDiffer = Activator.CreateInstance(typeof(Diffster<>).MakeGenericType(property.PropertyType), _formatter);
                    var diffMethod = nestedDiffer.GetType().GetMethod("Diff");
                    var nestedDifferences = (List<PropertyDifference>)diffMethod.Invoke(nestedDiffer, new object[] { firstValue, secondValue, propertyPath });

                    differences.AddRange(nestedDifferences);
                }
            }
            else if (!Equals(firstValue, secondValue))
            {
                differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
            }
        }

        return differences;
    }

    private List<PropertyDifference> CompareCollections(IEnumerable first, IEnumerable second, string propertyPath)
    {
        var differences = new List<PropertyDifference>();

        if (first == null || second == null)
        {
            if (!Equals(first, second))
            {
                differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = first, SecondValue = second });
            }
            return differences;
        }

        var firstList = first.Cast<object>().ToList();
        var secondList = second.Cast<object>().ToList();

        int maxLength = Math.Max(firstList.Count, secondList.Count);

        for (int i = 0; i < maxLength; i++)
        {
            string indexedPath = $"{propertyPath}[{i}]";

            if (i >= firstList.Count)
            {
                differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = null, SecondValue = secondList[i] });
            }
            else if (i >= secondList.Count)
            {
                differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = firstList[i], SecondValue = null });
            }
            else if (IsEnum(firstList[i].GetType()))
            {
                if (!Equals(firstList[i], secondList[i]))
                {
                    differences.Add(new PropertyDifference
                    {
                        PropertyName = indexedPath,
                        FirstValue = Enum.GetName(firstList[i].GetType(), firstList[i]),
                        SecondValue = Enum.GetName(firstList[i].GetType(), secondList[i])
                    });
                }
            }
            else if (IsComplexType(firstList[i].GetType()) || IsStruct(firstList[i].GetType()))
            {
                var nestedDiffer = Activator.CreateInstance(typeof(Diffster<>).MakeGenericType(firstList[i].GetType()), _formatter);
                var diffMethod = nestedDiffer.GetType().GetMethod("Diff");
                var nestedDifferences = (List<PropertyDifference>)diffMethod.Invoke(nestedDiffer, new object[] { firstList[i], secondList[i], indexedPath });

                differences.AddRange(nestedDifferences);
            }
            else if (!Equals(firstList[i], secondList[i]))
            {
                differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = firstList[i], SecondValue = secondList[i] });
            }
        }

        return differences;
    }

    private bool IsEnum(Type type) => type.IsEnum;

    private bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal);

    private bool IsComplexType(Type type) => type.IsClass && type != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(type);

    private bool IsCollection(Type type) => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
}


