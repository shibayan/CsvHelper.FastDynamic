using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CsvHelper.FastDynamic
{
    public static class CsvWriterExtensions
    {
        public static void WriteDynamicRecords(this CsvWriter csvWriter, IEnumerable records)
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

        public static async Task WriteDynamicRecordsAsync(this CsvWriter csvWriter, IEnumerable records)
        {
            var context = csvWriter.Context;

            foreach (var record in records)
            {
                if (!context.HasHeaderBeenWritten && context.WriterConfiguration.HasHeaderRecord)
                {
                    csvWriter.WriteHeader(record);
                    await csvWriter.NextRecordAsync();
                }

                csvWriter.WriteFields(record);
                await csvWriter.NextRecordAsync();
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
