# Diffster

Diffster is a .NET library designed to compare two objects of the same type and return the differences in a specified format. It is highly customizable and can be integrated into various .NET applications.

## Features

- **Object Comparison**: Compare two objects of the same type and identify differences.
- **Customizable Output**: Format the differences using custom formatter functions.
- **Collection Support**: Compare collections and nested objects.
- **Integration with Dependency Injection**: Easily add Diffster services to your .NET applications using dependency injection.

## Usage

Here are some examples of how to use Diffster:
```csharp

var instance1 = new SampleClass1();
var instance2 = new SampleClass1();

instance1.NumberA = 1;
instance2.NumberA = 2;

var defaultDiff = instance1.Diff(instance2);

Output: NumberA;1;2

```

