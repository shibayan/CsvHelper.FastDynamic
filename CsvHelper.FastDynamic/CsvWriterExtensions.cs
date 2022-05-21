using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CsvHelper.FastDynamic;

public static class CsvWriterExtensions
{
    public static void WriteDynamicRecords(this CsvWriter csvWriter, IEnumerable<object> records)
    {
        var context = csvWriter.Context;
        var hasHeaderBeenWritten = false;

        foreach (var record in records)
        {
            if (!hasHeaderBeenWritten && context.Configuration.HasHeaderRecord)
            {
                csvWriter.WriteHeaderInternal(record);
                csvWriter.NextRecord();

                hasHeaderBeenWritten = true;
            }

            csvWriter.WriteRecordInternal(record);
            csvWriter.NextRecord();
        }
    }

    public static async Task WriteDynamicRecordsAsync(this CsvWriter csvWriter, IEnumerable<object> records)
    {
        var context = csvWriter.Context;
        var hasHeaderBeenWritten = false;

        foreach (var record in records)
        {
            if (!hasHeaderBeenWritten && context.Configuration.HasHeaderRecord)
            {
                csvWriter.WriteHeaderInternal(record);
                await csvWriter.NextRecordAsync().ConfigureAwait(false);

                hasHeaderBeenWritten = true;
            }

            csvWriter.WriteRecordInternal(record);
            await csvWriter.NextRecordAsync().ConfigureAwait(false);
        }
    }

#if NETSTANDARD2_1

    public static async Task WriteDynamicRecordsAsync(this CsvWriter csvWriter, IAsyncEnumerable<object> records)
    {
        var context = csvWriter.Context;
        var hasHeaderBeenWritten = false;

        await foreach (var record in records.ConfigureAwait(false))
        {
            if (!hasHeaderBeenWritten && context.Configuration.HasHeaderRecord)
            {
                csvWriter.WriteHeaderInternal(record);
                await csvWriter.NextRecordAsync().ConfigureAwait(false);

                hasHeaderBeenWritten = true;
            }

            csvWriter.WriteRecordInternal(record);
            await csvWriter.NextRecordAsync().ConfigureAwait(false);
        }
    }

#endif

    private static void WriteHeaderInternal(this CsvWriter csvWriter, object record)
    {
        switch (record)
        {
            case IReadOnlyDictionary<string, object> dictionary:
            {
                var fieldNames = dictionary.Keys;

                if (csvWriter.Configuration.DynamicPropertySort is not null)
                {
                    fieldNames = fieldNames.OrderBy(x => x, csvWriter.Configuration.DynamicPropertySort);
                }

                foreach (var fieldName in fieldNames)
                {
                    csvWriter.WriteField(fieldName);
                }

                break;
            }
            case IDynamicMetaObjectProvider dynamicObject:
                csvWriter.WriteDynamicHeader(dynamicObject);
                break;
            default:
                csvWriter.WriteHeader(record.GetType());
                break;
        }
    }

    private static void WriteRecordInternal(this CsvWriter csvWriter, object record)
    {
        switch (record)
        {
            case IReadOnlyDictionary<string, object> dictionary:
            {
                var fieldNames = dictionary.Keys;

                if (csvWriter.Configuration.DynamicPropertySort is not null)
                {
                    fieldNames = fieldNames.OrderBy(x => x, csvWriter.Configuration.DynamicPropertySort);
                }

                foreach (var fieldName in fieldNames)
                {
                    csvWriter.WriteField(dictionary[fieldName]);
                }

                break;
            }
            case IDynamicMetaObjectProvider dynamicObject:
                csvWriter.WriteRecord(dynamicObject);
                break;
            default:
                csvWriter.WriteRecord(record);
                break;
        }
    }
}
