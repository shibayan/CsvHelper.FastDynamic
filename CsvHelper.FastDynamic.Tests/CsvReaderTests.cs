using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace CsvHelper.FastDynamic.Tests
{
    public class CsvReaderTests
    {
        [Fact]
        public void ReadDynamicRecords()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords();

            int count = 0;

            foreach (var record in records)
            {
                Assert.NotNull(record);
                Assert.IsAssignableFrom<IDictionary<string, object>>(record);

                Assert.Equal(TestCsvRecords[count]["Id"], record.Id);
                Assert.Equal(TestCsvRecords[count]["Name"], record.Name);
                Assert.Equal(TestCsvRecords[count]["Location"], record.Location);

                count += 1;
            }

            Assert.Equal(3, count);
        }

        [Fact]
        public async Task ReadDynamicRecordsAsync()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecordsAsync();

            int count = 0;

            await foreach (var record in records)
            {
                Assert.NotNull(record);
                Assert.IsAssignableFrom<IDictionary<string, object>>(record);

                Assert.Equal(TestCsvRecords[count]["Id"], record.Id);
                Assert.Equal(TestCsvRecords[count]["Name"], record.Name);
                Assert.Equal(TestCsvRecords[count]["Location"], record.Location);

                count += 1;
            }

            Assert.Equal(3, count);
        }

        private CsvReader CreateInMemoryReader()
        {
            return new CsvReader(new StringReader(TestCsvContent), CultureInfo.InvariantCulture);
        }

        private const string TestCsvContent = @"Id,Name,Location
1,kazuakix,Wakayama
2,daruyanagi,Ehime
3,buchizo,Osaka
";

        private static readonly IReadOnlyList<IDictionary<string, string>> TestCsvRecords = new[]
        {
            new Dictionary<string, string> { { "Id", "1" }, { "Name", "kazuakix" }, { "Location", "Wakayama" } },
            new Dictionary<string, string> { { "Id", "2" }, { "Name", "daruyanagi" }, { "Location", "Ehime" } },
            new Dictionary<string, string> { { "Id", "3" }, { "Name", "buchizo" }, { "Location", "Osaka" } }
        };
    }
}
