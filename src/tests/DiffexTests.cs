﻿using System;
using System.Collections.Generic;

using Diffex;
using Diffex.Tests.TestObjects;

using Xunit;

namespace Diffex.Tests
{
    public class DiffexTests
    {
        [Fact]
        public void IntPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new IntPropertyClass { Id = 1 };
            var second = new IntPropertyClass { Id = 2 };
            var diffex = new Diffex<IntPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Id;1;2", result);
        }

        [Fact]
        public void DoublePropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new DoublePropertyClass { Value = 1.1 };
            var second = new DoublePropertyClass { Value = 2.2 };
            var diffex = new Diffex<DoublePropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Value;1.1;2.2", result);
        }

        [Fact]
        public void FloatPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new FloatPropertyClass { Value = 1.1f };
            var second = new FloatPropertyClass { Value = 2.2f };
            var diffex = new Diffex<FloatPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Value;1.1;2.2", result);
        }

        [Fact]
        public void BoolPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new BoolPropertyClass { IsTrue = true };
            var second = new BoolPropertyClass { IsTrue = false };
            var diffex = new Diffex<BoolPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("IsTrue;True;False", result);
        }

        [Fact]
        public void BytePropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new BytePropertyClass { Value = 1 };
            var second = new BytePropertyClass { Value = 2 };
            var diffex = new Diffex<BytePropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Value;1;2", result);
        }

        [Fact]
        public void StringPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new StringPropertyClass { Name = "First" };
            var second = new StringPropertyClass { Name = "Second" };
            var diffex = new Diffex<StringPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Name;First;Second", result);
        }

        [Fact]
        public void DateTimePropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new DateTimePropertyClass { Date = new DateTime(2020, 1, 1) };
            var second = new DateTimePropertyClass { Date = new DateTime(2021, 1, 1) };
            var diffex = new Diffex<DateTimePropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains($"Date;{first.Date};{second.Date}", result);
        }

        [Fact]
        public void EnumPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new EnumPropertyClass { Day = DayOfWeek.Monday };
            var second = new EnumPropertyClass { Day = DayOfWeek.Tuesday };
            var diffex = new Diffex<EnumPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Day;Monday;Tuesday", result);
        }

        [Fact]
        public void NestedPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new NestedPropertyClass
            {
                IntProperty = new IntPropertyClass { Id = 1 },
                DoubleProperty = new DoublePropertyClass { Value = 1.1 },
                BoolProperty = new BoolPropertyClass { IsTrue = true },
                StringProperty = new StringPropertyClass { Name = "First" },
                DateTimeProperty = new DateTimePropertyClass { Date = new DateTime(2020, 1, 1) },
                EnumProperty = new EnumPropertyClass { Day = DayOfWeek.Monday }
            };
            var second = new NestedPropertyClass
            {
                IntProperty = new IntPropertyClass { Id = 2 },
                DoubleProperty = new DoublePropertyClass { Value = 2.2 },
                BoolProperty = new BoolPropertyClass { IsTrue = false },
                StringProperty = new StringPropertyClass { Name = "Second" },
                DateTimeProperty = new DateTimePropertyClass { Date = new DateTime(2021, 1, 1) },
                EnumProperty = new EnumPropertyClass { Day = DayOfWeek.Tuesday }
            };
            var diffex = new Diffex<NestedPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("IntProperty.Id;1;2", result);
            Assert.Contains("DoubleProperty.Value;1.1;2.2", result);
            Assert.Contains("BoolProperty.IsTrue;True;False", result);
            Assert.Contains("StringProperty.Name;First;Second", result);
            Assert.Contains($"DateTimeProperty.Date;{first.DateTimeProperty.Date};{second.DateTimeProperty.Date}\r\n", result);
            Assert.Contains("EnumProperty.Day;Monday;Tuesday", result);
        }

        [Fact]
        public void ListPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new ListPropertyClass
            {
                IntList = new List<int> { 1, 2, 3 },
                StringList = new List<string> { "First", "Second" },
                DateTimeList = new List<DateTime> { new DateTime(2020, 1, 1) },
                EnumList = new List<DayOfWeek> { DayOfWeek.Monday }
            };
            var second = new ListPropertyClass
            {
                IntList = new List<int> { 4, 5, 6 },
                StringList = new List<string> { "Third", "Fourth" },
                DateTimeList = new List<DateTime> { new DateTime(2021, 1, 1) },
                EnumList = new List<DayOfWeek> { DayOfWeek.Tuesday }
            };
            var diffex = new Diffex<ListPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("IntList[0];1;4\r\nIntList[1];2;5\r\nIntList[2];3;6\r\n", result);
            Assert.Contains("StringList[0];First;Third\r\nStringList[1];Second;Fourth\r\n", result);
            Assert.Contains($"DateTimeList[0];{first.DateTimeList[0].Date};{second.DateTimeList[0].Date}\r\n", result);
            Assert.Contains("EnumList[0];Monday;Tuesday", result);
        }

        [Fact]
        public void DictionaryPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new DictionaryPropertyClass
            {
                IntStringDictionary = new Dictionary<int, string> { { 1, "First" } },
                StringDateTimeDictionary = new Dictionary<string, DateTime> { { "First", new DateTime(2020, 1, 1) } },
                EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Monday, 1 } }
            };
            var second = new DictionaryPropertyClass
            {
                IntStringDictionary = new Dictionary<int, string> { { 2, "Second" } },
                StringDateTimeDictionary = new Dictionary<string, DateTime> { { "Second", new DateTime(2021, 1, 1) } },
                EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Tuesday, 2 } }
            };
            var diffex = new Diffex<DictionaryPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("IntStringDictionary[0];[1, First];[2, Second]\r\n", result);
            Assert.Contains($"StringDateTimeDictionary[0];[First, {first.StringDateTimeDictionary["First"].Date}];[Second, {second.StringDateTimeDictionary["Second"].Date}]\r\n", result);
            Assert.Contains("EnumIntDictionary[0];[Monday, 1];[Tuesday, 2]", result);
        }

        [Fact]
        public void ComplexPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new ComplexPropertyClass
            {
                NestedProperty = new NestedPropertyClass
                {
                    IntProperty = new IntPropertyClass { Id = 1 },
                    DoubleProperty = new DoublePropertyClass { Value = 1.1 },
                    BoolProperty = new BoolPropertyClass { IsTrue = true },
                    StringProperty = new StringPropertyClass { Name = "First" },
                    DateTimeProperty = new DateTimePropertyClass { Date = new DateTime(2020, 1, 1) },
                    EnumProperty = new EnumPropertyClass { Day = DayOfWeek.Monday }
                },
                ListProperty = new ListPropertyClass
                {
                    IntList = new List<int> { 1, 2, 3 },
                    StringList = new List<string> { "First", "Second" },
                    DateTimeList = new List<DateTime> { new DateTime(2020, 1, 1) },
                    EnumList = new List<DayOfWeek> { DayOfWeek.Monday }
                },
                DictionaryProperty = new DictionaryPropertyClass
                {
                    IntStringDictionary = new Dictionary<int, string> { { 1, "First" } },
                    StringDateTimeDictionary = new Dictionary<string, DateTime> { { "First", new DateTime(2020, 1, 1) } },
                    EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Monday, 1 } }
                }
            };
            var second = new ComplexPropertyClass
            {
                NestedProperty = new NestedPropertyClass
                {
                    IntProperty = new IntPropertyClass { Id = 2 },
                    DoubleProperty = new DoublePropertyClass { Value = 2.2 },
                    BoolProperty = new BoolPropertyClass { IsTrue = false },
                    StringProperty = new StringPropertyClass { Name = "Second" },
                    DateTimeProperty = new DateTimePropertyClass { Date = new DateTime(2021, 1, 1) },
                    EnumProperty = new EnumPropertyClass { Day = DayOfWeek.Tuesday }
                },
                ListProperty = new ListPropertyClass
                {
                    IntList = new List<int> { 4, 5, 6 },
                    StringList = new List<string> { "Third", "Fourth" },
                    DateTimeList = new List<DateTime> { new DateTime(2021, 1, 1) },
                    EnumList = new List<DayOfWeek> { DayOfWeek.Tuesday }
                },
                DictionaryProperty = new DictionaryPropertyClass
                {
                    IntStringDictionary = new Dictionary<int, string> { { 2, "Second" } },
                    StringDateTimeDictionary = new Dictionary<string, DateTime> { { "Second", new DateTime(2021, 1, 1) } },
                    EnumIntDictionary = new Dictionary<DayOfWeek, int> { { DayOfWeek.Tuesday, 2 } }
                }
            };
            var diffex = new Diffex<ComplexPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("NestedProperty.IntProperty.Id;1;2", result);
            Assert.Contains("NestedProperty.DoubleProperty.Value;1.1;2.2", result);
            Assert.Contains("NestedProperty.BoolProperty.IsTrue;True;False", result);
            Assert.Contains("NestedProperty.StringProperty.Name;First;Second", result);
            Assert.Contains($"NestedProperty.DateTimeProperty.Date;{first.NestedProperty.DateTimeProperty.Date};{second.NestedProperty.DateTimeProperty.Date}", result);
            Assert.Contains("NestedProperty.EnumProperty.Day;Monday;Tuesday", result);
            Assert.Contains("ListProperty.IntList[0];1;4", result);
            Assert.Contains("ListProperty.StringList[0];First;Third", result);
            Assert.Contains($"ListProperty.DateTimeList[0];{first.ListProperty.DateTimeList[0].Date};{second.ListProperty.DateTimeList[0].Date}", result);
            Assert.Contains("ListProperty.EnumList[0];Monday;Tuesday", result);
            Assert.Contains("DictionaryProperty.IntStringDictionary[0];[1, First];[2, Second]", result);
            Assert.Contains($"DictionaryProperty.StringDateTimeDictionary[0];[First, {first.DictionaryProperty.StringDateTimeDictionary["First"].Date}];[Second, {second.DictionaryProperty.StringDateTimeDictionary["Second"].Date}]", result);
            Assert.Contains("DictionaryProperty.EnumIntDictionary[0];[Monday, 1];[Tuesday, 2]", result);
        }

        [Fact]
        public void ArrayPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new ArrayPropertyClass
            {
                IntArray = new int[] { 1, 2, 3 },
                StringArray = new string[] { "First", "Second" },
                DateTimeArray = new DateTime[] { new DateTime(2020, 1, 1) },
                EnumArray = new DayOfWeek[] { DayOfWeek.Monday }
            };
            var second = new ArrayPropertyClass
            {
                IntArray = new int[] { 4, 5, 6 },
                StringArray = new string[] { "Third", "Fourth" },
                DateTimeArray = new DateTime[] { new DateTime(2021, 1, 1) },
                EnumArray = new DayOfWeek[] { DayOfWeek.Tuesday }
            };
            var diffex = new Diffex<ArrayPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("IntArray[0];1;4", result);
            Assert.Contains("StringArray[0];First;Third", result);
            Assert.Contains($"DateTimeArray[0];{first.DateTimeArray[0]};{second.DateTimeArray[0]}", result);
            Assert.Contains("EnumArray[0];Monday;Tuesday", result);
        }

        [Fact]
        public void GenericPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new GenericPropertyClass<int> { Value = 1 };
            var second = new GenericPropertyClass<int> { Value = 2 };
            var diffex = new Diffex<GenericPropertyClass<int>, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Value;1;2", result);
        }

        [Fact]
        public void ConcretePropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new ConcretePropertyClass { Id = 1 };
            var second = new ConcretePropertyClass { Id = 2 };
            var diffex = new Diffex<ConcretePropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Id;1;2", result);
        }

        //[Fact]
        //public void StaticPropertyClass_Diff_ShouldIdentifyDifferences()
        //{
        //    StaticPropertyClass.Id = 1;
        //    var first = new StaticPropertyClass();
        //    StaticPropertyClass.Id = 2;
        //    var second = new StaticPropertyClass();
        //    var diffex = new Diffex<StaticPropertyClass, string>();

        //    var result = diffex.Diff(first, second);

        //    Assert.Contains("Id;1;2", result);
        //}

        [Fact]
        public void ReadOnlyPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new ReadOnlyPropertyClass(0);
            var second = new ReadOnlyPropertyClass(1);
            var diffex = new Diffex<ReadOnlyPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("Id;0;1", result);
        }

        //[Fact]
        //public void WriteOnlyPropertyClass_Diff_ShouldIdentifyDifferences()
        //{
        //    var first = new WriteOnlyPropertyClass();
        //    first.Id = 1;
        //    var second = new WriteOnlyPropertyClass();
        //    second.Id = 2;
        //    var diffex = new Diffex<WriteOnlyPropertyClass, string>();

        //    var result = diffex.Diff(first, second);

        //    Assert.Contains("Id;1;2", result);
        //}

        [Fact]
        public void IndexerPropertyClass_Diff_ShouldIdentifyDifferences()
        {
            var first = new IndexerPropertyClass(2);
            first[0] = 1;
            first[1] = 2;
            var second = new IndexerPropertyClass(2);
            second[0] = 1;
            second[1] = 1;
            var diffex = new Diffex<IndexerPropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("[1];2;1", result);
        }

        [Fact]
        public void PrivatePropertyClass_Diff_ShouldIdentifyDifferencesForPublicPropertiesOnly()
        {
            var first = new PrivatePropertyClass(2);
            first.IdPublic = 0;
            var second = new PrivatePropertyClass(3);
            second.IdPublic = 1;
            var diffex = new Diffex<PrivatePropertyClass, string>();

            var result = diffex.Diff(first, second);

            Assert.Contains("IdPublic;0;1", result);
        }
    }
}
