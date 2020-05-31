# CsvHelper.FastDynamic
 
![Build](https://github.com/shibayan/CsvHelper.FastDynamic/workflows/Build/badge.svg)
[![License](https://img.shields.io/github/license/shibayan/CsvHelper.FastDynamic.svg)](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)

Fast dynamic records reader and writer extensions for [CsvHelper](https://github.com/JoshClose/CsvHelper)

## NuGet Package

Package Name | Target Framework | NuGet
---|---|---
CsvHelper.FastDynamic | .NET Standard 2.0/2.1 | [![NuGet](https://img.shields.io/nuget/v/CsvHelper.FastDynamic.svg)](https://www.nuget.org/packages/CsvHelper.FastDynamic)

## Install

```
Install-Package CsvHelper.FastDynamic
```

```
dotnet add package CsvHelper.FastDynamic
```

## Usage

### Simple CSV Reader

```csharp
class Program
{
    static void Main(string[] args)
    {
        using var csvReader = new CsvReader(new StreamReader("sample.csv"), CultureInfo.InvariantCulture);

        var records = csvReader.GetDynamicRecords();

        foreach (var record in records)
        {
            Console.WriteLine(record);
        }
    }
}
```

### Async CSV Reader (.NET Standard 2.1 / C# 8.0)

```csharp
class Program
{
    static async Task Main(string[] args)
    {
        using var csvReader = new CsvReader(new StreamReader("sample.csv"), CultureInfo.InvariantCulture);

        var records = csvReader.GetDynamicRecordsAsync();

        await foreach (var record in records)
        {
            Console.WriteLine(record);
        }
    }
}
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
