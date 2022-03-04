# CsvHelper.FastDynamic

[![Build](https://github.com/shibayan/CsvHelper.FastDynamic/workflows/Build/badge.svg)](https://github.com/shibayan/CsvHelper.FastDynamic/actions/workflows/build.yml)
[![Downloads](https://badgen.net/nuget/dt/CsvHelper.FastDynamic)](https://www.nuget.org/packages/CsvHelper.FastDynamic/)
[![NuGet](https://badgen.net/nuget/v/CsvHelper.FastDynamic)](https://www.nuget.org/packages/CsvHelper.FastDynamic/)
[![License](https://badgen.net/github/license/shibayan/CsvHelper.FastDynamic)](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)

Fast dynamic CSV records reader and writer extensions for [CsvHelper](https://github.com/JoshClose/CsvHelper)

## Installation

```
Install-Package CsvHelper.FastDynamic
```

```
dotnet add package CsvHelper.FastDynamic
```

## Usage

### Simple CSV Reader

```csharp
using CsvHelper;
using CsvHelper.FastDynamic;

using var csvReader = new CsvReader(new StreamReader("sample.csv"), CultureInfo.InvariantCulture);

var records = csvReader.GetDynamicRecords();

foreach (var @record in records)
{
    Console.WriteLine(record);
}
```

### Async CSV Enumerate (.NET Standard 2.1 / C# 8.0)

```csharp
using CsvHelper;
using CsvHelper.FastDynamic;

using var csvReader = new CsvReader(new StreamReader("sample.csv"), CultureInfo.InvariantCulture);

var records = csvReader.EnumerateDynamicRecordsAsync();

await foreach (var @record in records)
{
    Console.WriteLine(record);
}
```

## Performance

### Dynamic record reader

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=6.0.200
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT


|               Method |     Mean |   Error |  StdDev |   Gen 0 |   Gen 1 | Allocated |
|--------------------- |---------:|--------:|--------:|--------:|--------:|----------:|
|           GetRecords | 834.0 us | 3.04 us | 2.85 us | 36.1328 | 17.5781 |    602 KB |
| GetDictionaryRecords | 247.4 us | 0.89 us | 0.78 us | 32.7148 | 16.1133 |    538 KB |
|    GetDynamicRecords | 227.5 us | 0.85 us | 0.80 us | 28.3203 | 13.4277 |    464 KB |
```

### Dynamic record writer

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=6.0.200
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT


|                            Method |     Mean |   Error |  StdDev |   Gen 0 |  Gen 1 | Allocated |
|---------------------------------- |---------:|--------:|--------:|--------:|-------:|----------:|
|        WriteRecords_DynamicObject | 819.2 us | 0.85 us | 0.76 us | 49.8047 | 9.7656 |    822 KB |
| WriteDynamicRecords_DynamicObject | 455.0 us | 2.22 us | 1.97 us |  7.8125 | 1.4648 |    134 KB |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
