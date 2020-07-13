using System.Collections.Generic;
using System.Linq;

namespace CsvHelper.FastDynamic.Performance.Internal
{
    internal static class CsvReaderExtension
    {
        internal static IReadOnlyList<IDictionary<string, object>> GetDictionaryRecords(this CsvReader csvReader)
        {
            // Read Header
            csvReader.Read();
            csvReader.ReadHeader();

            var headerRecord = csvReader.Context
                                        .HeaderRecord
                                        .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(x, i))
                                        .ToArray();

            var result = new List<IDictionary<string, object>>();

            while (csvReader.Read())
            {
                var record = new Dictionary<string, object>(headerRecord.Length);

                for (int i = 0; i < headerRecord.Length; i++)
                {
                    record[headerRecord[i]] = csvReader[i];
                }

                result.Add(record);
            }

            return result;
        }
    }
}
