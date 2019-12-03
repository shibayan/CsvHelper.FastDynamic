﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvHelper.FastDynamicReader
{
    public static class CsvReaderExtensions
    {
        public static IEnumerable<dynamic> GetRecords(this CsvReader csvReader)
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

            var csvTable = new CsvHeader(context.HeaderRecord
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

                    record = new CsvRecord(csvTable, values);
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
    }
}