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

### Dynamic records reader

```
BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2361/22H2/2022Update/SunValley2)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

|               Method |     Mean |   Error |  StdDev | Ratio |    Gen0 |    Gen1 | Allocated | Alloc Ratio |
|--------------------- |---------:|--------:|--------:|------:|--------:|--------:|----------:|------------:|
|           GetRecords | 679.9 μs | 2.30 μs | 2.04 μs |  1.00 | 31.2500 | 15.6250 | 510.87 KB |        1.00 |
| GetDictionaryRecords | 217.2 μs | 0.26 μs | 0.21 μs |  0.32 | 21.7285 | 21.4844 | 355.05 KB |        0.70 |
|    GetDynamicRecords | 159.6 μs | 1.12 μs | 1.04 μs |  0.23 | 14.4043 |  4.6387 | 237.29 KB |        0.46 |
|        GetRawRecords | 152.9 μs | 0.84 μs | 0.74 μs |  0.22 | 13.1836 |  5.1270 |    219 KB |        0.43 |
```

### Dynamic records writer

```
BenchmarkDotNet v0.13.7, Windows 11 (10.0.22621.2361/22H2/2022Update/SunValley2)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

|                            Method |     Mean |   Error |  StdDev | Ratio |    Gen0 |    Gen1 | Allocated | Alloc Ratio |
|---------------------------------- |---------:|--------:|--------:|------:|--------:|--------:|----------:|------------:|
|        WriteRecords_DynamicObject | 659.9 μs | 1.74 μs | 1.54 μs |  1.00 | 55.6641 | 12.6953 | 914.65 KB |        1.00 |
| WriteDynamicRecords_DynamicObject | 391.7 μs | 1.78 μs | 1.66 μs |  0.59 | 13.6719 |  2.9297 | 225.95 KB |        0.25 |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
