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

### Reader

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i9-10940X CPU 3.30GHz, 1 CPU, 28 logical and 14 physical cores
.NET Core SDK=3.1.301
  [Host]     : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT


|               Method |       Mean |    Error |   StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|--------------------- |-----------:|---------:|---------:|--------:|--------:|------:|----------:|
|           GetRecords | 1,687.1 us | 17.11 us | 15.17 us | 83.9844 | 41.0156 |     - | 829.18 KB |
| GetDictionaryRecords |   776.7 us | 14.59 us | 16.21 us | 73.2422 | 34.1797 |     - | 725.63 KB |
|    GetDynamicRecords |   617.3 us | 11.23 us |  9.96 us | 61.5234 |  2.9297 |     - | 608.87 KB |
```

### Writer

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i9-10940X CPU 3.30GHz, 1 CPU, 28 logical and 14 physical cores
.NET Core SDK=3.1.301
  [Host]     : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT


|              Method |       Mean |    Error |   StdDev |    Gen 0 |   Gen 1 | Gen 2 |  Allocated |
|-------------------- |-----------:|---------:|---------:|---------:|--------:|------:|-----------:|
|        WriteRecords | 2,355.6 us | 46.86 us | 64.14 us | 148.4375 | 27.3438 |     - | 1476.46 KB |
| WriteDynamicRecords |   885.7 us | 17.50 us | 16.37 us |  17.5781 |  2.9297 |     - |  174.49 KB |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
