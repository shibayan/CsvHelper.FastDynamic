using System.Collections.Generic;

namespace CsvHelper.FastDynamic.Tests
{
    public static class TestData
    {
        public static readonly string CsvContent = "Id,Name,Location\r\n1,kazuakix,Wakayama\r\n2,daruyanagi,Ehime\r\n3,buchizo,Osaka\r\n";

        public static readonly IReadOnlyList<IDictionary<string, string>> CsvRecords = new[]
        {
            new Dictionary<string, string> { { "Id", "1" }, { "Name", "kazuakix" }, { "Location", "Wakayama" } },
            new Dictionary<string, string> { { "Id", "2" }, { "Name", "daruyanagi" }, { "Location", "Ehime" } },
            new Dictionary<string, string> { { "Id", "3" }, { "Name", "buchizo" }, { "Location", "Osaka" } }
        };

        public static readonly IReadOnlyList<object> CsvAnonymousRecords = new[]
        {
            new { Id = 1, Name = "kazuakix", Location = "Wakayama" },
            new { Id = 2, Name = "daruyanagi", Location = "Ehime" },
            new { Id = 3, Name = "buchizo", Location = "Osaka" }
        };
    }
}
