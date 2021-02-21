using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsvHelper.FastDynamic
{
    public static class CsvReaderExtensions
    {
        public static IReadOnlyList<dynamic> GetDynamicRecords(this CsvReader csvReader)
            => csvReader.EnumerateDynamicRecords().ToArray();

        public static IEnumerable<dynamic> EnumerateDynamicRecords(this CsvReader csvReader)
        {
            if (csvReader.Configuration.HasHeaderRecord && csvReader.HeaderRecord == null)
            {
                if (!csvReader.Read())
                {
                    yield break;
                }

                csvReader.ReadHeader();
            }

            var csvHeader = new CsvHeader(csvReader.HeaderRecord
                                                   .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(new PrepareHeaderForMatchArgs(x, i)))
                                                   .ToArray());

            while (csvReader.Read())
            {
                CsvRecord record;

                try
                {
                    var values = new object[csvReader.HeaderRecord.Length];

                    for (int i = 0; i < csvReader.HeaderRecord.Length; i++)
                    {
                        values[i] = csvReader.Parser[i];
                    }

                    record = new CsvRecord(csvHeader, values);
                }
                catch (Exception ex)
                {
                    var readerException = new ReaderException(csvReader.Context, "An unexpected error occurred.", ex);

                    if (csvReader.Configuration.ReadingExceptionOccurred?.Invoke(new ReadingExceptionOccurredArgs(readerException)) ?? true)
                    {
                        throw readerException;
                    }

                    continue;
                }

                yield return record;
            }
        }

#if NETSTANDARD2_1

        public static async Task<IReadOnlyList<dynamic>> GetDynamicRecordsAsync(this CsvReader csvReader)
        {
            var records = new List<dynamic>();

            await foreach (var record in csvReader.EnumerateDynamicRecordsAsync())
            {
                records.Add(record);
            }

            return records;
        }

        public static async IAsyncEnumerable<dynamic> EnumerateDynamicRecordsAsync(this CsvReader csvReader)
        {
            if (csvReader.Configuration.HasHeaderRecord && csvReader.HeaderRecord == null)
            {
                if (!await csvReader.ReadAsync().ConfigureAwait(false))
                {
                    yield break;
                }

                csvReader.ReadHeader();
            }

            var csvHeader = new CsvHeader(csvReader.HeaderRecord
                                                   .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(new PrepareHeaderForMatchArgs(x, i)))
                                                   .ToArray());

            while (await csvReader.ReadAsync().ConfigureAwait(false))
            {
                CsvRecord record;

                try
                {
                    var values = new object[csvReader.HeaderRecord.Length];

                    Array.Copy(csvReader.Parser.Record, values, csvReader.Parser.Record.Length);

                    record = new CsvRecord(csvHeader, values);
                }
                catch (Exception ex)
                {
                    var readerException = new ReaderException(csvReader.Context, "An unexpected error occurred.", ex);

                    if (csvReader.Configuration.ReadingExceptionOccurred?.Invoke(new ReadingExceptionOccurredArgs(readerException)) ?? true)
                    {
                        throw readerException;
                    }

                    continue;
                }

                yield return record;
            }
        }

#endif
    }
}
