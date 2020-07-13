using System.Collections.Generic;
using System.Globalization;
using System.IO;

using BenchmarkDotNet.Attributes;

namespace CsvHelper.FastDynamic.Performance
{
    [MemoryDiagnoser]
    public class WriterBenchmark
    {
        private const string SampleCsvFile = @".\sampledata\SFO_Airport_Monthly_Utility_Consumption_for_Natural_Gas__Water__and_Electricity.csv";

        public WriterBenchmark()
        {
            using var csvReader = new CsvReader(new StreamReader(SampleCsvFile), CultureInfo.InvariantCulture);

            _sampleCsvData = csvReader.GetDynamicRecords();
        }

        private readonly IReadOnlyList<dynamic> _sampleCsvData;

        [Benchmark]
        public void WriteRecords()
        {
            using var csvWriter = new CsvWriter(new StringWriter(), CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(_sampleCsvData);
        }

        [Benchmark]
        public void WriteDynamicRecords()
        {
            using var csvWriter = new CsvWriter(new StringWriter(), CultureInfo.InvariantCulture);

            csvWriter.WriteDynamicRecords(_sampleCsvData);
        }
    }
}
