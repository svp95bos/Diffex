
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

public class Diffster<T, TOutput>
{
    private readonly IDiffFormatter<TOutput> _formatter;

    public Diffster(IDiffFormatter<TOutput> formatter = null)
    {
        _formatter = formatter ?? (IDiffFormatter<TOutput>)new DefaultDiffFormatter();
    }

    public TOutput Diff(T first, T second)
    {
        var differences = GetDifferences(first, second);
        return _formatter.Format(differences);
    }

    public List<PropertyDifference> GetDifferences(T first, T second, string parentPath = "")
    {
        if (first == null || second == null)
        {
            throw new ArgumentNullException("Both instances must be non-null.");
        }

        List<PropertyDifference> differences = new List<PropertyDifference>();
        Type type = typeof(T);

        if (type.IsPrimitive || type.IsValueType || type == typeof(string))
        {
            if (!Equals(first, second))
            {
                differences.Add(new PropertyDifference { PropertyName = parentPath, FirstValue = first, SecondValue = second });
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
                    var nestedDiffer = new Diffster<object, List<PropertyDifference>>();
                    var nestedDifferences = nestedDiffer.GetDifferences(firstValue, secondValue, propertyPath);
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
            else if (IsComplexType(firstList[i].GetType()) || IsStruct(firstList[i].GetType()))
            {
                var nestedDiffer = new Diffster<object, List<PropertyDifference>>();
                var nestedDifferences = nestedDiffer.GetDifferences(firstList[i], secondList[i], indexedPath);
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


