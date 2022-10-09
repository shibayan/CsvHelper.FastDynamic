using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvHelper.FastDynamic.Performance.Internal;

internal static class CsvReaderExtension
{
    internal static IReadOnlyList<IDictionary<string, object>> GetDictionaryRecords(this CsvReader csvReader)
        => csvReader.EnumerateDictionaryRecords().ToArray();

    internal static IEnumerable<IDictionary<string, object>> EnumerateDictionaryRecords(this CsvReader csvReader)
    {
        if (csvReader.Configuration.HasHeaderRecord && csvReader.HeaderRecord is null)
        {
            if (!csvReader.Read())
            {
                yield break;
            }

            csvReader.ReadHeader();
        }

        var headerRecord = csvReader.HeaderRecord
                                    .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(new PrepareHeaderForMatchArgs(x, i)))
                                    .ToArray();

        while (csvReader.Read())
        {
            Dictionary<string, object> record;

            try
            {
                record = new Dictionary<string, object>(headerRecord.Length);

                for (var i = 0; i < headerRecord.Length; i++)
                {
                    record[headerRecord[i]] = csvReader.Parser[i];
                }
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

    internal static IReadOnlyList<string[]> GetRawRecords(this CsvReader csvReader)
        => csvReader.EnumerateRawRecords().ToArray();

    internal static IEnumerable<string[]> EnumerateRawRecords(this CsvReader csvReader)
    {
        if (csvReader.Configuration.HasHeaderRecord && csvReader.HeaderRecord is null)
        {
            if (!csvReader.Read())
            {
                yield break;
            }

            csvReader.ReadHeader();
        }

        while (csvReader.Read())
        {
            string[] record;

            try
            {
                record = csvReader.Parser.Record;
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
}
