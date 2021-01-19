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
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i9-10940X CPU 3.30GHz, 1 CPU, 28 logical and 14 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT


|               Method |       Mean |    Error |   StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|--------------------- |-----------:|---------:|---------:|--------:|--------:|------:|----------:|
|           GetRecords | 1,350.7 us | 26.83 us | 38.48 us | 78.1250 | 39.0625 |     - | 785.81 KB |
| GetDictionaryRecords |   419.0 us |  8.24 us | 11.81 us | 53.2227 | 20.0195 |     - |  526.7 KB |
|    GetDynamicRecords |   331.0 us |  6.59 us | 10.45 us | 41.5039 |  7.3242 |     - | 408.92 KB |
```

### Writer

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i9-10940X CPU 3.30GHz, 1 CPU, 28 logical and 14 physical cores
.NET Core SDK=5.0.102
  [Host]     : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT
  DefaultJob : .NET Core 5.0.2 (CoreCLR 5.0.220.61120, CoreFX 5.0.220.61120), X64 RyuJIT


|              Method |       Mean |    Error |    StdDev |    Gen 0 |   Gen 1 | Gen 2 |  Allocated |
|-------------------- |-----------:|---------:|----------:|---------:|--------:|------:|-----------:|
|        WriteRecords | 4,104.7 us | 81.42 us | 142.60 us | 359.3750 | 70.3125 |     - | 3532.62 KB |
| WriteDynamicRecords |   714.8 us | 14.16 us |  25.17 us |  12.6953 |  1.9531 |     - |  132.48 KB |
```

## Thanks

- [CsvHelper](https://github.com/JoshClose/CsvHelper) by @JoshClose
- [Dapper](https://github.com/StackExchange/Dapper) by @StackExchange

## License

This project is licensed under the [Apache License 2.0](https://github.com/shibayan/CsvHelper.FastDynamic/blob/master/LICENSE)
