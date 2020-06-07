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

                Assert.Equal(TestData.CsvRecords[count]["Id"], record.Id);
                Assert.Equal(TestData.CsvRecords[count]["Name"], record.Name);
                Assert.Equal(TestData.CsvRecords[count]["Location"], record.Location);

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

                Assert.Equal(TestData.CsvRecords[count]["Id"], record.Id);
                Assert.Equal(TestData.CsvRecords[count]["Name"], record.Name);
                Assert.Equal(TestData.CsvRecords[count]["Location"], record.Location);

                count += 1;
            }

            Assert.Equal(3, count);
        }

        private CsvReader CreateInMemoryReader()
        {
            return new CsvReader(new StringReader(TestData.CsvContent), CultureInfo.InvariantCulture);
        }
    }
}
