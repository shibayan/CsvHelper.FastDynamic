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

### Async CSV Enumerate (.NET Standard 2.1 / C# 8.0 or later)

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
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.608)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


|               Method |     Mean |   Error |  StdDev | Ratio |    Gen0 |    Gen1 | Allocated | Alloc Ratio |
|--------------------- |---------:|--------:|--------:|------:|--------:|--------:|----------:|------------:|
|           GetRecords | 878.3 us | 3.62 us | 3.20 us |  1.00 | 31.2500 | 15.6250 | 510.84 KB |        1.00 |
| GetDictionaryRecords | 208.0 us | 0.85 us | 0.76 us |  0.24 | 21.7285 | 10.7422 | 355.03 KB |        0.69 |
|    GetDynamicRecords | 176.4 us | 1.07 us | 0.95 us |  0.20 | 14.4043 |  6.3477 | 237.26 KB |        0.46 |
|        GetRawRecords | 154.7 us | 1.08 us | 1.01 us |  0.18 | 13.1836 |  5.8594 | 218.98 KB |        0.43 |
```

### Dynamic record writer

```
BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.608)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


|                            Method |     Mean |   Error |  StdDev | Ratio |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
|---------------------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
|        WriteRecords_DynamicObject | 873.9 us | 2.82 us | 2.64 us |  1.00 | 55.6641 | 9.7656 | 914.53 KB |        1.00 |
| WriteDynamicRecords_DynamicObject | 498.9 us | 2.52 us | 2.36 us |  0.57 | 13.6719 | 2.4414 | 225.84 KB |        0.25 |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
