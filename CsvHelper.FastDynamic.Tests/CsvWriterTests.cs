using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace CsvHelper.FastDynamic.Tests
{
    public class CsvWriterTests
    {
        [Fact]
        public void WriteDynamicObjects()
        {
            var (csvWriter, stringWriter) = CreateInMemoryWriter();

            var records = new[]
            {
                new Dictionary<string, object> { { "Id", 1 }, { "Name", "kazuakix" }, { "Location", "Wakayama" } },
                new Dictionary<string, object> { { "Id", 2 }, { "Name", "daruyanagi" }, { "Location", "Ehime" } },
                new Dictionary<string, object> { { "Id", 3 }, { "Name", "buchizo" }, { "Location", "Osaka" } }
            };

            var dynamicRecords = records.Select(ConvertToExpandoObject)
                                        .ToArray();

            csvWriter.WriteDynamicRecords(dynamicRecords);

            Assert.Equal(TestCsvContent, stringWriter.ToString());
        }

        [Fact]
        public async Task WriteDynamicObjectsAsync()
        {
            var (csvWriter, stringWriter) = CreateInMemoryWriter();

            var records = new[]
            {
                new Dictionary<string, object> { { "Id", 1 }, { "Name", "kazuakix" }, { "Location", "Wakayama" } },
                new Dictionary<string, object> { { "Id", 2 }, { "Name", "daruyanagi" }, { "Location", "Ehime" } },
                new Dictionary<string, object> { { "Id", 3 }, { "Name", "buchizo" }, { "Location", "Osaka" } }
            };

            var dynamicRecords = records.Select(ConvertToExpandoObject)
                                        .ToArray();

            await csvWriter.WriteDynamicRecordsAsync(dynamicRecords);

            Assert.Equal(TestCsvContent, stringWriter.ToString());
        }

        [Fact]
        public void WriteAnonymousObjects()
        {
            var (csvWriter, stringWriter) = CreateInMemoryWriter();

            var records = new[]
            {
                new { Id = 1, Name = "kazuakix", Location = "Wakayama" },
                new { Id = 2, Name = "daruyanagi", Location = "Ehime" },
                new { Id = 3, Name = "buchizo", Location = "Osaka" }
            };

            csvWriter.WriteDynamicRecords(records);

            Assert.Equal(TestCsvContent, stringWriter.ToString());
        }

        [Fact]
        public async Task WriteAnonymousObjectsAsync()
        {
            var (csvWriter, stringWriter) = CreateInMemoryWriter();

            var records = new[]
            {
                new { Id = 1, Name = "kazuakix", Location = "Wakayama" },
                new { Id = 2, Name = "daruyanagi", Location = "Ehime" },
                new { Id = 3, Name = "buchizo", Location = "Osaka" }
            };

            await csvWriter.WriteDynamicRecordsAsync(records);

            Assert.Equal(TestCsvContent, stringWriter.ToString());
        }

        private (CsvWriter, StringWriter) CreateInMemoryWriter()
        {
            var stringWriter = new StringWriter();

            return (new CsvWriter(stringWriter, CultureInfo.InvariantCulture), stringWriter);
        }

        private ExpandoObject ConvertToExpandoObject(IDictionary<string, object> dictionary)
        {
            var expandoObject = new ExpandoObject();

            foreach (var item in dictionary)
            {
                expandoObject.TryAdd(item.Key, item.Value);
            }

            return expandoObject;
        }

        private const string TestCsvContent = @"Id,Name,Location
1,kazuakix,Wakayama
2,daruyanagi,Ehime
3,buchizo,Osaka
";
    }
}
