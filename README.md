# CsvHelper.FastDynamic

[![Build](https://github.com/shibayan/CsvHelper.FastDynamic/workflows/Build/badge.svg)](https://github.com/shibayan/CsvHelper.FastDynamic/actions/workflows/build.yml)
[![Downloads](https://badgen.net/nuget/dt/CsvHelper.FastDynamic)](https://www.nuget.org/packages/CsvHelper.FastDynamic/)
[![NuGet](https://badgen.net/nuget/v/CsvHelper.FastDynamic)](https://www.nuget.org/packages/CsvHelper.FastDynamic/)
[![License](https://badgen.net/github/license/shibayan/CsvHelper.FastDynamic)](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)

Fast dynamic records reader and writer extensions for [CsvHelper](https://github.com/JoshClose/CsvHelper)

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
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1387 (21H2)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|               Method |     Mean |   Error |  StdDev |   Gen 0 |   Gen 1 | Allocated |
|--------------------- |---------:|--------:|--------:|--------:|--------:|----------:|
|           GetRecords | 868.6 us | 3.65 us | 3.23 us | 36.1328 | 17.5781 |    602 KB |
| GetDictionaryRecords | 254.7 us | 1.35 us | 1.19 us | 32.7148 | 16.1133 |    538 KB |
|    GetDynamicRecords | 210.5 us | 1.07 us | 1.00 us | 25.6348 | 11.7188 |    420 KB |
```

### Writer

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1387 (21H2)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|                               Method |     Mean |   Error |  StdDev |   Gen 0 |  Gen 1 | Allocated |
|------------------------------------- |---------:|--------:|--------:|--------:|-------:|----------:|
|           WriteRecords_DynamicObject | 818.5 us | 3.12 us | 2.92 us | 49.8047 | 9.7656 |    822 KB |
|    WriteDynamicRecords_DynamicObject | 445.7 us | 2.05 us | 1.82 us |  7.8125 | 1.4648 |    134 KB |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
