using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace CsvHelper.FastDynamic.Tests;

public class CsvWriterTests
{
    [Fact]
    public void WriteDynamicObjects()
    {
        var (csvWriter, stringWriter) = CreateInMemoryWriter();

        var records = TestData.CsvRecords;

        var dynamicRecords = records.Select(ConvertToExpandoObject)
                                    .ToArray();

        csvWriter.WriteDynamicRecords(dynamicRecords);

        Assert.Equal(TestData.CsvContent, stringWriter.ToString());
    }

    [Fact]
    public async Task WriteDynamicObjectsAsync()
    {
        var (csvWriter, stringWriter) = CreateInMemoryWriter();

        var records = TestData.CsvRecords;

        var dynamicRecords = records.Select(ConvertToExpandoObject)
                                    .ToArray();

        await csvWriter.WriteDynamicRecordsAsync(dynamicRecords);

        Assert.Equal(TestData.CsvContent, stringWriter.ToString());
    }

    [Fact]
    public void WriteAnonymousObjects()
    {
        var (csvWriter, stringWriter) = CreateInMemoryWriter();

        var records = TestData.CsvAnonymousRecords;

        csvWriter.WriteDynamicRecords(records);

        Assert.Equal(TestData.CsvContent, stringWriter.ToString());
    }

    [Fact]
    public async Task WriteAnonymousObjectsAsync()
    {
        var (csvWriter, stringWriter) = CreateInMemoryWriter();

        var records = TestData.CsvAnonymousRecords;

        await csvWriter.WriteDynamicRecordsAsync(records);

        Assert.Equal(TestData.CsvContent, stringWriter.ToString());
    }

    private (CsvWriter, StringWriter) CreateInMemoryWriter()
    {
        var stringWriter = new StringWriter();

        return (new CsvWriter(stringWriter, CultureInfo.InvariantCulture), stringWriter);
    }

    private ExpandoObject ConvertToExpandoObject(IDictionary<string, string> dictionary)
    {
        var expandoObject = new ExpandoObject();

        foreach (var item in dictionary)
        {
            expandoObject.TryAdd(item.Key, item.Value);
        }

        return expandoObject;
    }
}
