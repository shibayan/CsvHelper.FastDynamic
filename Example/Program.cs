using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.FastDynamic;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var csvReader = new CsvReader(new StreamReader("sample.csv"), CultureInfo.InvariantCulture);

            var records = csvReader.GetDynamicRecordsAsync();

            await foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }
    }
}
