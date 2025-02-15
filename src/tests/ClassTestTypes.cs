using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffex.Tests.TestObjects;

public class IntPropertyClass
{
    public int Id { get; set; }
}

public class DoublePropertyClass
{
    public double Value { get; set; }
}

public class FloatPropertyClass
{
    public float Value { get; set; }
}

public class BoolPropertyClass
{
    public bool IsTrue { get; set; }
}

public class BytePropertyClass
{
    public byte Value { get; set; }
}

public class StringPropertyClass
{
    public string Name { get; set; }
}

public class DateTimePropertyClass
{
    public DateTime Date { get; set; }
}

public class EnumPropertyClass
{
    public DayOfWeek Day { get; set; }
}

public class NestedPropertyClass
{
    public IntPropertyClass IntProperty { get; set; }
    public DoublePropertyClass DoubleProperty { get; set; }
    public BoolPropertyClass BoolProperty { get; set; }
    public StringPropertyClass StringProperty { get; set; }
    public DateTimePropertyClass DateTimeProperty { get; set; }
    public EnumPropertyClass EnumProperty { get; set; }
}

public class ListPropertyClass
{
    public List<int> IntList { get; set; }
    public List<string> StringList { get; set; }
    public List<DateTime> DateTimeList { get; set; }
    public List<DayOfWeek> EnumList { get; set; }
}

public class DictionaryPropertyClass
{
    public Dictionary<int, string> IntStringDictionary { get; set; }
    public Dictionary<string, DateTime> StringDateTimeDictionary { get; set; }
    public Dictionary<DayOfWeek, int> EnumIntDictionary { get; set; }
}

public class ComplexPropertyClass
{
    public NestedPropertyClass NestedProperty { get; set; }
    public ListPropertyClass ListProperty { get; set; }
    public DictionaryPropertyClass DictionaryProperty { get; set; }
}

public class ArrayPropertyClass
{
    public int[] IntArray { get; set; }
    public string[] StringArray { get; set; }
    public DateTime[] DateTimeArray { get; set; }
    public DayOfWeek[] EnumArray { get; set; }
}

public class GenericPropertyClass<T>
{
    public T Value { get; set; }
}

public abstract class AbstractPropertyClass
{
    public abstract int Id { get; set; }
}

public class ConcretePropertyClass : AbstractPropertyClass
{
    public override int Id { get; set; }
}

public class StaticPropertyClass
{
    public static int Id { get; set; }
}

public class ReadOnlyPropertyClass(int id)
{
    public int Id { get; init; } = id;
}

public class WriteOnlyPropertyClass
{
    private int _id;
    public int Id { set { _id = value;  } }
}

public class IndexerPropertyClass
{
    private List<int> _list;

    public IndexerPropertyClass(int size)
    {
        _list = new List<int>(size);
        for (int i = 0; i < size; i++)
        {
            _list.Add(0); // Initialize with default values
        }
    }

    public int this[int index]
    {
        get { return _list[index]; }
        set { _list[index] = value; }
    }
}


public class PrivatePropertyClass(int privateId)
{
    private int PrivateId { get; init; } = privateId;
    public int IdPublic { get; set; }
}







