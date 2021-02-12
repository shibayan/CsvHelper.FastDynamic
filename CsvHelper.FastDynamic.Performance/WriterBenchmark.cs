using System.Collections.Generic;
using System.Globalization;
using System.IO;

using BenchmarkDotNet.Attributes;

using CsvHelper.FastDynamic.Performance.Internal;

namespace CsvHelper.FastDynamic.Performance
{
    [MemoryDiagnoser]
    public class WriterBenchmark
    {
        private const string SampleCsvFile = @".\sampledata\SFO_Airport_Monthly_Utility_Consumption_for_Natural_Gas__Water__and_Electricity.csv";

        public WriterBenchmark()
        {
            using (var csvReader = new CsvReader(new StreamReader(SampleCsvFile), CultureInfo.InvariantCulture))
            {
                _dynamicCsvData = csvReader.GetDynamicRecords();
            }

            using (var csvReader = new CsvReader(new StreamReader(SampleCsvFile), CultureInfo.InvariantCulture))
            {
                _dictionaryCsvData = csvReader.GetDictionaryRecords();
            }
        }

        private readonly IReadOnlyList<dynamic> _dynamicCsvData;
        private readonly IReadOnlyList<IDictionary<string, object>> _dictionaryCsvData;

        [Benchmark]
        public void WriteRecords_DynamicObject()
        {
            using var csvWriter = new CsvWriter(new StringWriter(), CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(_dynamicCsvData);
        }

        [Benchmark]
        public void WriteDynamicRecords_DynamicObject()
        {
            using var csvWriter = new CsvWriter(new StringWriter(), CultureInfo.InvariantCulture);

            csvWriter.WriteDynamicRecords(_dynamicCsvData);
        }

        [Benchmark]
        public void WriteRecords_DictionaryObject()
        {
            using var csvWriter = new CsvWriter(new StringWriter(), CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(_dictionaryCsvData);
        }

        [Benchmark]
        public void WriteDynamicRecords_DictionaryObject()
        {
            using var csvWriter = new CsvWriter(new StringWriter(), CultureInfo.InvariantCulture);

            csvWriter.WriteDynamicRecords(_dictionaryCsvData);
        }
    }
}
