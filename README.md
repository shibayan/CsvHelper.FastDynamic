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

### Async CSV Enumerate (.NET Standard 2.1 / C# 8.0)

```csharp
class Program
{
    static async Task Main(string[] args)
    {
        using var csvReader = new CsvReader(new StreamReader("sample.csv"), CultureInfo.InvariantCulture);

        var records = csvReader.EnumerateDynamicRecordsAsync();

        await foreach (var record in records)
        {
            Console.WriteLine(record);
        }
    }
}
```

## Performance

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.264 (2004/?/20H1)
Intel Core i9-10940X CPU 3.30GHz, 1 CPU, 28 logical and 14 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  DefaultJob : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT


|               Method |       Mean |    Error |   StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|--------------------- |-----------:|---------:|---------:|--------:|--------:|------:|----------:|
|           GetRecords | 1,586.6 us | 12.29 us | 11.50 us | 83.9844 | 41.0156 |     - | 828.13 KB |
| GetDictionaryRecords |   734.3 us |  7.96 us |  7.45 us | 73.2422 | 35.1563 |     - | 728.99 KB |
|    GetDynamicRecords |   599.2 us |  5.04 us |  4.47 us | 61.5234 |  1.9531 |     - | 607.82 KB |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
