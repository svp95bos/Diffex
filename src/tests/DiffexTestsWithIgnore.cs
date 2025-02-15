using System;
using System.Collections.Generic;
using Diffex;
using Diffex.Tests.TestObjects;
using Xunit;

namespace Diffex.Tests
{
    public class DiffexTestsWithIgnore
    {
        [Fact]
        public void IntPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new IntPropertyClassWithIgnore { Id = 1 };
            var second = new IntPropertyClassWithIgnore { Id = 2 };
            var diffex = new Diffex<IntPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Id", result);
        }

        [Fact]
        public void DoublePropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new DoublePropertyClassWithIgnore { Value = 1.1 };
            var second = new DoublePropertyClassWithIgnore { Value = 2.2 };
            var diffex = new Diffex<DoublePropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Value", result);
        }

        [Fact]
        public void FloatPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new FloatPropertyClassWithIgnore { Value = 1.1f };
            var second = new FloatPropertyClassWithIgnore { Value = 2.2f };
            var diffex = new Diffex<FloatPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Value", result);
        }

        [Fact]
        public void BoolPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new BoolPropertyClassWithIgnore { IsTrue = true };
            var second = new BoolPropertyClassWithIgnore { IsTrue = false };
            var diffex = new Diffex<BoolPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("IsTrue", result);
        }

        [Fact]
        public void BytePropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new BytePropertyClassWithIgnore { Value = 1 };
            var second = new BytePropertyClassWithIgnore { Value = 2 };
            var diffex = new Diffex<BytePropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Value", result);
        }

        [Fact]
        public void StringPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new StringPropertyClassWithIgnore { Name = "First" };
            var second = new StringPropertyClassWithIgnore { Name = "Second" };
            var diffex = new Diffex<StringPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Name", result);
        }

        [Fact]
        public void DateTimePropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new DateTimePropertyClassWithIgnore { Date = new DateTime(2020, 1, 1) };
            var second = new DateTimePropertyClassWithIgnore { Date = new DateTime(2021, 1, 1) };
            var diffex = new Diffex<DateTimePropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Date", result);
        }

        [Fact]
        public void EnumPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new EnumPropertyClassWithIgnore { Day = DayOfWeek.Monday };
            var second = new EnumPropertyClassWithIgnore { Day = DayOfWeek.Tuesday };
            var diffex = new Diffex<EnumPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Day", result);
        }

        [Fact]
        public void NestedPropertyClassWithIgnore_Diff_ShouldIgnoreProperties()
        {
            var first = new NestedPropertyClassWithIgnore
            {
                IntProperty = new IntPropertyClassWithIgnore { Id = 1 },
                DoubleProperty = new DoublePropertyClassWithIgnore { Value = 1.1 },
                BoolProperty = new BoolPropertyClassWithIgnore { IsTrue = true },
                StringProperty = new StringPropertyClassWithIgnore { Name = "First" },
                DateTimeProperty = new DateTimePropertyClassWithIgnore { Date = new DateTime(2020, 1, 1) },
                EnumProperty = new EnumPropertyClassWithIgnore { Day = DayOfWeek.Monday }
            };
            var second = new NestedPropertyClassWithIgnore
            {
                IntProperty = new IntPropertyClassWithIgnore { Id = 2 },
                DoubleProperty = new DoublePropertyClassWithIgnore { Value = 2.2 },
                BoolProperty = new BoolPropertyClassWithIgnore { IsTrue = false },
                StringProperty = new StringPropertyClassWithIgnore { Name = "Second" },
                DateTimeProperty = new DateTimePropertyClassWithIgnore { Date = new DateTime(2021, 1, 1) },
                EnumProperty = new EnumPropertyClassWithIgnore { Day = DayOfWeek.Tuesday }
            };
            var diffex = new Diffex<NestedPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("IntProperty.Id", result);
            Assert.DoesNotContain("DoubleProperty.Value", result);
            Assert.DoesNotContain("BoolProperty.IsTrue", result);
            Assert.DoesNotContain("StringProperty.Name", result);
            Assert.DoesNotContain("DateTimeProperty.Date", result);
            Assert.DoesNotContain("EnumProperty.Day", result);
        }

        [Fact]
        public void ListPropertyClassWithIgnore_Diff_ShouldIgnoreProperties()
        {
            var first = new ListPropertyClassWithIgnore
            {
                IntList = new List<int> { 1, 2, 3 },
                StringList = new List<string> { "First", "Second" },
                DateTimeList = new List<DateTime> { new DateTime(2020, 1, 1) },
                EnumList = new List<DayOfWeek> { DayOfWeek.Monday }
            };
            var second = new ListPropertyClassWithIgnore
            {
                IntList = new List<int> { 4, 5, 6 },
                StringList = new List<string> { "Third", "Fourth" },
                DateTimeList = new List<DateTime> { new DateTime(2021, 1, 1) },
                EnumList = new List<DayOfWeek> { DayOfWeek.Tuesday }
            };
            var diffex = new Diffex<ListPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("IntList", result);
            Assert.DoesNotContain("StringList", result);
            Assert.DoesNotContain("DateTimeList", result);
            Assert.DoesNotContain("EnumList", result);
        }

        [Fact]
        public void DictionaryPropertyClassWithIgnore_Diff_ShouldIgnoreProperties()
        {
            var first = new DictionaryPropertyClassWithIgnore
            {
                IntStringDictionary = new Dictionary<int, string> { { 1, "First" } },
                StringDateTimeDictionary = new Dictionary<string, DateTime> { { "First", new DateTime(2020, 1, 1) } },
                EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Monday, 1 } }
            };
            var second = new DictionaryPropertyClassWithIgnore
            {
                IntStringDictionary = new Dictionary<int, string> { { 2, "Second" } },
                StringDateTimeDictionary = new Dictionary<string, DateTime> { { "Second", new DateTime(2021, 1, 1) } },
                EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Tuesday, 2 } }
            };
            var diffex = new Diffex<DictionaryPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("IntStringDictionary", result);
            Assert.DoesNotContain("StringDateTimeDictionary", result);
            Assert.DoesNotContain("EnumIntDictionary", result);
        }

        [Fact]
        public void ComplexPropertyClassWithIgnore_Diff_ShouldIgnoreProperties()
        {
            var first = new ComplexPropertyClassWithIgnore
            {
                NestedProperty = new NestedPropertyClassWithIgnore
                {
                    IntProperty = new IntPropertyClassWithIgnore { Id = 1 },
                    DoubleProperty = new DoublePropertyClassWithIgnore { Value = 1.1 },
                    BoolProperty = new BoolPropertyClassWithIgnore { IsTrue = true },
                    StringProperty = new StringPropertyClassWithIgnore { Name = "First" },
                    DateTimeProperty = new DateTimePropertyClassWithIgnore { Date = new DateTime(2020, 1, 1) },
                    EnumProperty = new EnumPropertyClassWithIgnore { Day = DayOfWeek.Monday }
                },
                ListProperty = new ListPropertyClassWithIgnore
                {
                    IntList = new List<int> { 1, 2, 3 },
                    StringList = new List<string> { "First", "Second" },
                    DateTimeList = new List<DateTime> { new DateTime(2020, 1, 1) },
                    EnumList = new List<DayOfWeek> { DayOfWeek.Monday }
                },
                DictionaryProperty = new DictionaryPropertyClassWithIgnore
                {
                    IntStringDictionary = new Dictionary<int, string> { { 1, "First" } },
                    StringDateTimeDictionary = new Dictionary<string, DateTime> { { "First", new DateTime(2020, 1, 1) } },
                    EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Monday, 1 } }
                }
            };
            var second = new ComplexPropertyClassWithIgnore
            {
                NestedProperty = new NestedPropertyClassWithIgnore
                {
                    IntProperty = new IntPropertyClassWithIgnore { Id = 2 },
                    DoubleProperty = new DoublePropertyClassWithIgnore { Value = 2.2 },
                    BoolProperty = new BoolPropertyClassWithIgnore { IsTrue = false },
                    StringProperty = new StringPropertyClassWithIgnore { Name = "Second" },
                    DateTimeProperty = new DateTimePropertyClassWithIgnore { Date = new DateTime(2021, 1, 1) },
                    EnumProperty = new EnumPropertyClassWithIgnore { Day = DayOfWeek.Tuesday }
                },
                ListProperty = new ListPropertyClassWithIgnore
                {
                    IntList = new List<int> { 4, 5, 6 },
                    StringList = new List<string> { "Third", "Fourth" },
                    DateTimeList = new List<DateTime> { new DateTime(2021, 1, 1) },
                    EnumList = new List<DayOfWeek> { DayOfWeek.Tuesday }
                },
                DictionaryProperty = new DictionaryPropertyClassWithIgnore
                {
                    IntStringDictionary = new Dictionary<int, string> { { 2, "Second" } },
                    StringDateTimeDictionary = new Dictionary<string, DateTime> { { "Second", new DateTime(2021, 1, 1) } },
                    EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Tuesday, 2 } }
                }
            };
            var diffex = new Diffex<ComplexPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("NestedProperty", result);
            Assert.DoesNotContain("ListProperty", result);
            Assert.DoesNotContain("DictionaryProperty", result);
        }

        [Fact]
        public void ArrayPropertyClassWithIgnore_Diff_ShouldIgnoreProperties()
        {
            var first = new ArrayPropertyClassWithIgnore
            {
                IntArray = new int[] { 1, 2, 3 },
                StringArray = new string[] { "First", "Second" },
                DateTimeArray = new DateTime[] { new DateTime(2020, 1, 1) },
                EnumArray = new DayOfWeek[] { DayOfWeek.Monday }
            };
            var second = new ArrayPropertyClassWithIgnore
            {
                IntArray = new int[] { 4, 5, 6 },
                StringArray = new string[] { "Third", "Fourth" },
                DateTimeArray = new DateTime[] { new DateTime(2021, 1, 1) },
                EnumArray = new DayOfWeek[] { DayOfWeek.Tuesday }
            };
            var diffex = new Diffex<ArrayPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("IntArray", result);
            Assert.DoesNotContain("StringArray", result);
            Assert.DoesNotContain("DateTimeArray", result);
            Assert.DoesNotContain("EnumArray", result);
        }

        [Fact]
        public void GenericPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new GenericPropertyClassWithIgnore<int> { Value = 1 };
            var second = new GenericPropertyClassWithIgnore<int> { Value = 2 };
            var diffex = new Diffex<GenericPropertyClassWithIgnore<int>, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Value", result);
        }

        [Fact]
        public void ConcretePropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new ConcretePropertyClassWithIgnore { Id = 1 };
            var second = new ConcretePropertyClassWithIgnore { Id = 2 };
            var diffex = new Diffex<ConcretePropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Id", result);
        }

        [Fact]
        public void ReadOnlyPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new ReadOnlyPropertyClassWithIgnore(0);
            var second = new ReadOnlyPropertyClassWithIgnore(1);
            var diffex = new Diffex<ReadOnlyPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("Id", result);
        }

        [Fact]
        public void IndexerPropertyClassWithIgnore_Diff_ShouldIgnoreProperty()
        {
            var first = new IndexerPropertyClassWithIgnore(2);
            first[0] = 1;
            first[1] = 2;
            var second = new IndexerPropertyClassWithIgnore(2);
            second[0] = 1;
            second[1] = 1;
            var diffex = new Diffex<IndexerPropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("[1]", result);
        }

        [Fact]
        public void PrivatePropertyClassWithIgnore_Diff_ShouldIgnorePrivateProperty()
        {
            var first = new PrivatePropertyClassWithIgnore(2);
            first.IdPublic = 0;
            first.IdPublic_Ignored = 2;
            var second = new PrivatePropertyClassWithIgnore(3);
            second.IdPublic = 1;
            second.IdPublic_Ignored = 3;
            var diffex = new Diffex<PrivatePropertyClassWithIgnore, string>();

            var result = diffex.Diff(first, second);

            Assert.DoesNotContain("IdPublic_Ignored", result);
            Assert.Contains("IdPublic;0;1", result);
        }
    }
}
