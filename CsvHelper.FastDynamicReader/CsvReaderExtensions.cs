using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvHelper.FastDynamicReader
{
    public static class CsvReaderExtensions
    {
        public static IEnumerable<dynamic> GetRecords(this CsvReader csvReader)
        {
            if (csvReader.Context.ReaderConfiguration.HasHeaderRecord && csvReader.Context.HeaderRecord == null)
            {
                if (!csvReader.Read())
                {
                    yield break;
                }

                csvReader.ReadHeader();
            }

            var csvTable = new CsvHeader(csvReader.Context
                                                  .HeaderRecord
                                                  .Select((x, i) => csvReader.Configuration.PrepareHeaderForMatch(x, i))
                                                  .ToArray());

            while (csvReader.Read())
            {
                CsvRecord record;

                try
                {
                    var values = new object[csvReader.Context.HeaderRecord.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = csvReader.GetField(i);
                    }

                    record = new CsvRecord(csvTable, values);
                }
                catch (Exception ex)
                {
                    var readerException = new ReaderException(csvReader.Context, "An unexpected error occurred.", ex);

                    if (csvReader.Context.ReaderConfiguration.ReadingExceptionOccurred?.Invoke(readerException) ?? true)
                    {
                        throw readerException;
                    }

                    continue;
                }

                yield return record;
            }
        }
    }
}