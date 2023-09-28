using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsvHelper.FastDynamic;

public static class CsvReaderExtensions
{
    public static IReadOnlyList<dynamic> GetDynamicRecords(this CsvReader csvReader) => csvReader.EnumerateDynamicRecords().ToArray();

    public static IEnumerable<dynamic> EnumerateDynamicRecords(this CsvReader csvReader)
    {
        if (csvReader.Configuration.HasHeaderRecord && csvReader.HeaderRecord is null)
        {
            if (!csvReader.Read())
            {
                yield break;
            }

            csvReader.ReadHeader();
        }

        if (!csvReader.Read())
        {
            yield break;
        }

        var csvHeader = new CsvHeader(Enumerable.Range(0, csvReader.HeaderRecord?.Length ?? csvReader.Parser.Count)
                                                .Select((_, i) => csvReader.Configuration.GetDynamicPropertyName(new GetDynamicPropertyNameArgs(i, csvReader.Context)))
                                                .ToArray());

        do
        {
            CsvRecord record;

            try
            {
                var values = new object[csvHeader.FieldNames.Length];

                for (var i = 0; i < csvHeader.FieldNames.Length; i++)
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

        } while (csvReader.Read());
    }

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
        if (csvReader.Configuration.HasHeaderRecord && csvReader.HeaderRecord is null)
        {
            if (!await csvReader.ReadAsync().ConfigureAwait(false))
            {
                yield break;
            }

            csvReader.ReadHeader();
        }

        if (!await csvReader.ReadAsync().ConfigureAwait(false))
        {
            yield break;
        }

        var csvHeader = new CsvHeader(Enumerable.Range(0, csvReader.HeaderRecord?.Length ?? csvReader.Parser.Count)
                                                .Select((_, i) => csvReader.Configuration.GetDynamicPropertyName(new GetDynamicPropertyNameArgs(i, csvReader.Context)))
                                                .ToArray());

        do
        {
            CsvRecord record;

            try
            {
                var values = new object[csvHeader.FieldNames.Length];

                for (var i = 0; i < csvHeader.FieldNames.Length; i++)
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

        } while (await csvReader.ReadAsync().ConfigureAwait(false));
    }
}
