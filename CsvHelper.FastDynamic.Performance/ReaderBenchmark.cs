using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using BenchmarkDotNet.Attributes;

using CsvHelper.FastDynamic.Performance.Internal;

namespace CsvHelper.FastDynamic.Performance;

[MemoryDiagnoser]
public class ReaderBenchmark
{
    private const string SampleCsvFile = @".\sampledata\SFO_Airport_Monthly_Utility_Consumption_for_Natural_Gas__Water__and_Electricity.csv";

    public ReaderBenchmark()
    {
        _sampleCsvData = File.ReadAllText(SampleCsvFile);
    }

    private readonly string _sampleCsvData;

    [Benchmark]
    public IReadOnlyList<dynamic> GetRecords()
    {
        using var csvReader = new CsvReader(new StringReader(_sampleCsvData), CultureInfo.InvariantCulture);

        return csvReader.GetRecords<dynamic>().ToArray();
    }

    [Benchmark]
    public IReadOnlyList<IDictionary<string, object>> GetDictionaryRecords()
    {
        using var csvReader = new CsvReader(new StringReader(_sampleCsvData), CultureInfo.InvariantCulture);

        return csvReader.GetDictionaryRecords();
    }

    [Benchmark]
    public IReadOnlyList<dynamic> GetDynamicRecords()
    {
        using var csvReader = new CsvReader(new StringReader(_sampleCsvData), CultureInfo.InvariantCulture);

        return csvReader.GetDynamicRecords();
    }
}
