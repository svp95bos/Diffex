# 🔍 Differ<T> - A Generic Object Comparison Library

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![C#](https://img.shields.io/badge/language-C%23-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![GitHub stars](https://img.shields.io/github/stars/your-repo.svg?style=social&label=Star)](https://github.com/your-repo)

## Introduction

**Differ<T>** is a powerful and flexible C# library for comparing two instances of a given object type (`T`). It supports **deep comparison**, **list/array differences**, **primitive types**, **structs**, **enums**, and **complex nested objects**. This library is designed for developers who need an efficient way to identify differences between objects in .NET applications.

---

## 🚀 Features

✅ **Deep Object Comparison** – Detects differences in **nested objects** and **lists**  
✅ **Collection & Array Support** – Compares lists, arrays, and other **IEnumerable** types  
✅ **Struct & Enum Support** – Correctly handles **value types and enumerations**  
✅ **Formatted Differences** – Outputs results in a structured format for easy analysis  
✅ **Lightweight & Fast** – Uses reflection efficiently without unnecessary overhead  

---

## 📦 Installation

To use Differ<T>, simply clone or download this repository. You can also add it to your .NET project as a class library.

```sh
git clone https://github.com/your-repo/differ-t.git
```

If you're using **.NET CLI**, you can include it in your project manually.

---

## 🛠 Usage

### Basic Comparison

```csharp
var differ = new Differ<Person>();
var differences = person1.Diff(person2);

foreach (var diff in differences)
{
    Console.WriteLine(diff);
}
```

### Example Output
```
Age: '30' vs '31'
Address.Street: '123 Main St' vs '456 Elm St'
Nicknames[1]: 'Ace' vs 'Queen'
PreviousAddresses[1].Street: '222 Second St' vs '333 Third St'
PreviousAddresses[1].City: 'Boston' vs 'Seattle'
```

### Supported Data Types
- **Primitive types** (`int`, `float`, `double`, `string`, etc.)
- **Structs & Value Types**
- **Enums** (`Status.Active` vs. `Status.Inactive`)
- **Complex Objects**
- **Lists & Arrays** (`List<T>`, `T[]`)

---

## 📌 Advanced Features

### Ignore Specific Properties
If you want to ignore specific properties during comparison, you can extend the `Differ<T>` class with filtering logic.

### Custom Formatting
Implement `IDiffFormatter` to customize how differences are displayed.

Example:
```csharp
public class JsonDiffFormatter : IDiffFormatter
{
    public string Format(PropertyDifference difference)
    {
        return $"{{ \"property\": \"{difference.PropertyName}\", \"old\": \"{difference.FirstValue}\", \"new\": \"{difference.SecondValue}\" }}";
    }
}
```

---

## 🏗 Contributing
Pull requests are welcome! If you'd like to contribute:
1. Fork this repository
2. Create a new branch (`feature-xyz`)
3. Commit changes and push to your fork
4. Open a Pull Request

---

## 📜 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## ⭐ Support & Feedback
If you find this library useful, please ⭐ star this repo! Feel free to submit issues, suggestions, or feature requests.

📧 Contact: [your-email@example.com]  
🔗 GitHub: [github.com/your-repo](https://github.com/your-repo)

