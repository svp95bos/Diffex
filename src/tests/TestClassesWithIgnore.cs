using System;
using System.Collections.Generic;

namespace Diffex.Tests.TestObjects;

public class IntPropertyClassWithIgnore
{
    [DiffexIgnore]
    public int Id { get; set; }
}

public class DoublePropertyClassWithIgnore
{
    [DiffexIgnore]
    public double Value { get; set; }
}

public class FloatPropertyClassWithIgnore
{
    [DiffexIgnore]
    public float Value { get; set; }
}

public class BoolPropertyClassWithIgnore
{
    [DiffexIgnore]
    public bool IsTrue { get; set; }
}

public class BytePropertyClassWithIgnore
{
    [DiffexIgnore]
    public byte Value { get; set; }
}

public class StringPropertyClassWithIgnore
{
    [DiffexIgnore]
    public string Name { get; set; }
}

public class DateTimePropertyClassWithIgnore
{
    [DiffexIgnore]
    public DateTime Date { get; set; }
}

public class EnumPropertyClassWithIgnore
{
    [DiffexIgnore]
    public DayOfWeek Day { get; set; }
}

public class NestedPropertyClassWithIgnore
{
    [DiffexIgnore]
    public IntPropertyClassWithIgnore IntProperty { get; set; }
    [DiffexIgnore]
    public DoublePropertyClassWithIgnore DoubleProperty { get; set; }
    [DiffexIgnore]
    public BoolPropertyClassWithIgnore BoolProperty { get; set; }
    [DiffexIgnore]
    public StringPropertyClassWithIgnore StringProperty { get; set; }
    [DiffexIgnore]
    public DateTimePropertyClassWithIgnore DateTimeProperty { get; set; }
    [DiffexIgnore]
    public EnumPropertyClassWithIgnore EnumProperty { get; set; }
}

public class ListPropertyClassWithIgnore
{
    [DiffexIgnore]
    public List<int> IntList { get; set; }
    [DiffexIgnore]
    public List<string> StringList { get; set; }
    [DiffexIgnore]
    public List<DateTime> DateTimeList { get; set; }
    [DiffexIgnore]
    public List<DayOfWeek> EnumList { get; set; }
}

public class DictionaryPropertyClassWithIgnore
{
    [DiffexIgnore]
    public Dictionary<int, string> IntStringDictionary { get; set; }
    [DiffexIgnore]
    public Dictionary<string, DateTime> StringDateTimeDictionary { get; set; }
    [DiffexIgnore]
    public Dictionary<DayOfWeek, int> EnumIntDictionary { get; set; }
}

public class ComplexPropertyClassWithIgnore
{
    [DiffexIgnore]
    public NestedPropertyClassWithIgnore NestedProperty { get; set; }
    [DiffexIgnore]
    public ListPropertyClassWithIgnore ListProperty { get; set; }
    [DiffexIgnore]
    public DictionaryPropertyClassWithIgnore DictionaryProperty { get; set; }
}

public class ArrayPropertyClassWithIgnore
{
    [DiffexIgnore]
    public int[] IntArray { get; set; }
    [DiffexIgnore]
    public string[] StringArray { get; set; }
    [DiffexIgnore]
    public DateTime[] DateTimeArray { get; set; }
    [DiffexIgnore]
    public DayOfWeek[] EnumArray { get; set; }
}

public class GenericPropertyClassWithIgnore<T>
{
    [DiffexIgnore]
    public T Value { get; set; }
}

public abstract class AbstractPropertyClassWithIgnore
{
    [DiffexIgnore]
    public abstract int Id { get; set; }
}

public class ConcretePropertyClassWithIgnore : AbstractPropertyClassWithIgnore
{
    [DiffexIgnore]
    public override int Id { get; set; }
}

public class StaticPropertyClassWithIgnore
{
    [DiffexIgnore]
    public static int Id { get; set; }
}

public class ReadOnlyPropertyClassWithIgnore(int id)
{
    [DiffexIgnore]
    public int Id { get; init; } = id;
}

public class WriteOnlyPropertyClassWithIgnore
{
    private int _id;
    [DiffexIgnore]
    public int Id { set { _id = value;  } }
}

public class IndexerPropertyClassWithIgnore
{
    private List<int> _list;

    public IndexerPropertyClassWithIgnore(int size)
    {
        _list = new List<int>(size);
        for (int i = 0; i < size; i++)
        {
            _list.Add(0); // Initialize with default values
        }
    }

    [DiffexIgnore]
    public int this[int index]
    {
        get { return _list[index]; }
        set { _list[index] = value; }
    }
}

public class PrivatePropertyClassWithIgnore(int privateId)
{

    [DiffexIgnore]
    private int PrivateId { get; init; } = privateId;

    [DiffexIgnore]
    public int IdPublic_Ignored { get; set; }

    public int IdPublic { get; set; }
}
