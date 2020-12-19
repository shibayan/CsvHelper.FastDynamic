using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace CsvHelper.FastDynamic.Tests
{
    public class CsvReaderTests
    {
        [Fact]
        public void ReadDynamicRecords_Member()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords();

            Assert.NotNull(records);
            Assert.Equal(3, records.Count);

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i].Id);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i].Name);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i].Location);
            }
        }

        [Fact]
        public void ReadDynamicRecords_Indexer()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords();

            Assert.NotNull(records);
            Assert.Equal(3, records.Count);

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i]["Id"]);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i]["Name"]);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i]["Location"]);
            }
        }

        [Fact]
        public void ReadDynamicRecords_Dictionary()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords()
                                   .Cast<IDictionary<string, object>>()
                                   .ToArray();

            Assert.NotNull(records);
            Assert.Equal(3, records.Length);

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i]["Id"]);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i]["Name"]);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i]["Location"]);
            }
        }

        [Fact]
        public async Task ReadDynamicRecordsAsync()
        {
            var csvReader = CreateInMemoryReader();

            var records = await csvReader.GetDynamicRecordsAsync();

            Assert.NotNull(records);
            Assert.Equal(3, records.Count);

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i].Id);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i].Name);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i].Location);
            }
        }

        [Fact]
        public void EnumerateDynamicRecords()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.EnumerateDynamicRecords();

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
        public async Task EnumerateDynamicRecordsAsync()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.EnumerateDynamicRecordsAsync();

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

        [Fact]
        public void AddDynamicColumns_Member()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords();

            for (int i = 0; i < 3; i++)
            {
                records[i].Append = $"test-{i}";
            }

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i].Id);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i].Name);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i].Location);

                Assert.Equal($"test-{i}", records[i].Append);
            }
        }

        [Fact]
        public void AddDynamicColumns_Indexer()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords();

            for (int i = 0; i < 3; i++)
            {
                records[i]["Append"] = $"test-{i}";
            }

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i]["Id"]);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i]["Name"]);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i]["Location"]);

                Assert.Equal($"test-{i}", records[i]["Append"]);
            }
        }

        [Fact]
        public void AddDynamicColumns_Dictionary()
        {
            var csvReader = CreateInMemoryReader();

            var records = csvReader.GetDynamicRecords()
                                   .Cast<IDictionary<string, object>>()
                                   .ToArray();

            for (int i = 0; i < 3; i++)
            {
                records[i]["Append"] = $"test-{i}";
            }

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(TestData.CsvRecords[i]["Id"], records[i]["Id"]);
                Assert.Equal(TestData.CsvRecords[i]["Name"], records[i]["Name"]);
                Assert.Equal(TestData.CsvRecords[i]["Location"], records[i]["Location"]);

                Assert.Equal($"test-{i}", records[i]["Append"]);
            }
        }

        private CsvReader CreateInMemoryReader()
        {
            return new CsvReader(new StringReader(TestData.CsvContent), CultureInfo.InvariantCulture);
        }
    }
}
