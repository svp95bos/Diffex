﻿using System.Collections.Generic;
using Xunit;

namespace Diffster.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_PrimitiveTypes()
    {
        int first = 1;
        int second = 2;
        var result = first.Diff(second);
        Assert.Contains("null;1;2", result);
    }

    [Fact]
    public void Test_PrimitiveTypesNoDiff()
    {
        int first = 1;
        int second = 1;
        var result = first.Diff(second);
        Assert.Contains("", result);
    }

    [Fact]
    public void Test_ValueTypes()
    {
        DateTime first = new DateTime(2020, 1, 1);
        DateTime second = new DateTime(2021, 1, 1);
        var result = first.Diff(second);
        Assert.Contains("null;2020-01-01 00:00:00;2021-01-01 00:00:00", result);
    }

    [Fact]
    public void Test_ValueTypesNoDiff()
    {
        DateTime first = new DateTime(2020, 1, 1);
        DateTime second = new DateTime(2020, 1, 1);
        var result = first.Diff(second);
        Assert.Contains("", result);
    }

    [Fact]
    public void Test_String()
    {
        string first = "Hello";
        string second = "World";
        var result = first.Diff(second);
        Assert.Contains("null;Hello;World", result);
    }

    [Fact]
    public void Test_Enum()
    {
        DayOfWeek first = DayOfWeek.Monday;
        DayOfWeek second = DayOfWeek.Tuesday;
        var result = first.Diff(second);
        Assert.Contains("null;Monday;Tuesday", result);
    }

    [Fact]
    public void Test_EnumNoDiff()
    {
        DayOfWeek first = DayOfWeek.Monday;
        DayOfWeek second = DayOfWeek.Monday;
        var result = first.Diff(second);
        Assert.Contains("", result);
    }

    [Fact]
    public void Test_Collections()
    {
        List<int> first = new List<int> { 1, 2, 3 };
        List<int> second = new List<int> { 1, 2, 4 };
        var result = first.Diff(second);
        Assert.Contains("[2];3;4", result);
    }

    [Fact]
    public void Test_ComplexTypesSinglePropertyDiff()
    {
        var first = new { Name = "Alice", Age = 30 };
        var second = new { Name = "Bob", Age = 30 };
        var result = first.Diff(second);
        Assert.Contains("Name;Alice;Bob", result);
    }

    [Fact]
    public void Test_ComplexTypesMultiplePropertyDiff()
    {
        var first = new { Name = "Alice", Age = 30 };
        var second = new { Name = "Bob", Age = 31 };
        var result = first.Diff(second);
        Assert.Contains("Name;Alice;Bob\r\nAge;30;31", result);
    }

    [Fact]
    public void Test_ComplexTypesNoDiff()
    {
        var first = new { Name = "Alice", Age = 30 };
        var second = new { Name = "Alice", Age = 30 };
        var result = first.Diff(second);
        Assert.Contains("", result);
    }

    [Fact]
    public void Test_CustomFormatter()
    {
        var first = new { Name = "Alice", Age = 30 };
        var second = new { Name = "Bob", Age = 30 };
        var result = first.Diff(second, differences => string.Join(", ", differences.Select(d => $"{d.PropertyName}: {d.FirstValue} -> {d.SecondValue}")));
        Assert.Contains("Name: Alice -> Bob", result);
    }
}
