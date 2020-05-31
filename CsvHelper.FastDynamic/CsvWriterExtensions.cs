using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CsvHelper.FastDynamic
{
    public static class CsvWriterExtensions
    {
        public static void WriteDynamicRecords(this CsvWriter csvWriter, IEnumerable<object> records)
        {
            var context = csvWriter.Context;

            foreach (var record in records)
            {
                if (!context.HasHeaderBeenWritten && context.WriterConfiguration.HasHeaderRecord)
                {
                    csvWriter.WriteHeader(record);
                    csvWriter.NextRecord();
                }

                csvWriter.WriteFields(record);
                csvWriter.NextRecord();
            }
        }

        public static async Task WriteDynamicRecordsAsync(this CsvWriter csvWriter, IAsyncEnumerable<object> records)
        {
            var context = csvWriter.Context;

            await foreach (var record in records.ConfigureAwait(false))
            {
                if (!context.HasHeaderBeenWritten && context.WriterConfiguration.HasHeaderRecord)
                {
                    csvWriter.WriteHeader(record);
                    await csvWriter.NextRecordAsync().ConfigureAwait(false);
                }

                csvWriter.WriteFields(record);
                await csvWriter.NextRecordAsync().ConfigureAwait(false);
            }
        }

        private static void WriteHeader(this CsvWriter csvWriter, object record)
        {
            if (record is IReadOnlyDictionary<string, object> dictionary)
            {
                var fieldNames = dictionary.Keys;

                if (csvWriter.Configuration.DynamicPropertySort != null)
                {
                    fieldNames = fieldNames.OrderBy(x => x, csvWriter.Configuration.DynamicPropertySort);
                }

                foreach (var fieldName in fieldNames)
                {
                    csvWriter.WriteField(fieldName);
                }

                csvWriter.Context.HasHeaderBeenWritten = true;
            }
            else if (record is IDynamicMetaObjectProvider dynamicObject)
            {
                csvWriter.WriteDynamicHeader(dynamicObject);
            }
            else
            {
                throw new ArgumentException("Not supported element type.");
            }
        }

        private static void WriteFields(this CsvWriter csvWriter, object record)
        {
            if (record is IReadOnlyDictionary<string, object> dictionary)
            {
                var fieldNames = dictionary.Keys;

                if (csvWriter.Configuration.DynamicPropertySort != null)
                {
                    fieldNames = fieldNames.OrderBy(x => x, csvWriter.Configuration.DynamicPropertySort);
                }

                foreach (var fieldName in fieldNames)
                {
                    csvWriter.WriteField(dictionary[fieldName]);
                }
            }
            else if (record is IDynamicMetaObjectProvider)
            {
                csvWriter.WriteRecord(record);
            }
            else
            {
                throw new ArgumentException("Not supported element type.");
            }
        }
    }
}
