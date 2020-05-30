using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvHelper.FastDynamic
{
    public static class CsvReaderExtensions
    {
        public static IEnumerable<dynamic> GetDynamicRecords(this CsvReader csvReader)
        {
            var context = csvReader.Context;

            if (context.ReaderConfiguration.HasHeaderRecord && context.HeaderRecord == null)
            {
                if (!csvReader.Read())
                {
                    yield break;
                }

                csvReader.ReadHeader();
            }

            var csvHeader = new CsvHeader(context.HeaderRecord
                                                 .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(x, i))
                                                 .ToArray());

            while (csvReader.Read())
            {
                CsvRecord record;

                try
                {
                    var values = new object[context.HeaderRecord.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = csvReader.GetField(i);
                    }

                    record = new CsvRecord(csvHeader, values);
                }
                catch (Exception ex)
                {
                    var readerException = new ReaderException(context, "An unexpected error occurred.", ex);

                    if (context.ReaderConfiguration.ReadingExceptionOccurred?.Invoke(readerException) ?? true)
                    {
                        throw readerException;
                    }

                    continue;
                }

                yield return record;
            }
        }

#if NETSTANDARD2_1

        public static async IAsyncEnumerable<dynamic> GetDynamicRecordsAsync(this CsvReader csvReader)
        {
            var context = csvReader.Context;

            if (context.ReaderConfiguration.HasHeaderRecord && context.HeaderRecord == null)
            {
                if (!await csvReader.ReadAsync())
                {
                    yield break;
                }

                csvReader.ReadHeader();
            }

            var csvHeader = new CsvHeader(context.HeaderRecord
                                                 .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(x, i))
                                                 .ToArray());

            while (await csvReader.ReadAsync())
            {
                CsvRecord record;

                try
                {
                    var values = new object[context.HeaderRecord.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = csvReader.GetField(i);
                    }

                    record = new CsvRecord(csvHeader, values);
                }
                catch (Exception ex)
                {
                    var readerException = new ReaderException(context, "An unexpected error occurred.", ex);

                    if (context.ReaderConfiguration.ReadingExceptionOccurred?.Invoke(readerException) ?? true)
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
