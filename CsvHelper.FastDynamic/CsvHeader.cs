using System;
using System.Collections.Generic;

namespace CsvHelper.FastDynamic;

// Thanks, from https://github.com/StackExchange/Dapper/blob/master/Dapper/SqlMapper.DapperTable.cs
internal sealed class CsvHeader
{
    private string[] _fieldNames;
    private readonly Dictionary<string, int> _fieldNameLookup;

    public string[] FieldNames => _fieldNames;

    public CsvHeader(string[] fieldNames)
    {
        _fieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));

        _fieldNameLookup = new Dictionary<string, int>(fieldNames.Length, StringComparer.Ordinal);

        for (var i = fieldNames.Length - 1; i >= 0; i--)
        {
            var name = fieldNames[i];

            if (name is not null)
            {
                _fieldNameLookup[name] = i;
            }
        }
    }

    public int IndexOfName(string name) => name is not null && _fieldNameLookup.TryGetValue(name, out var index) ? index : -1;

    public int AddField(string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (_fieldNameLookup.ContainsKey(name))
        {
            throw new InvalidOperationException($"Field already exists: {name}");
        }

        var oldLength = _fieldNames.Length;

        Array.Resize(ref _fieldNames, oldLength + 1);

        _fieldNames[oldLength] = name;
        _fieldNameLookup[name] = oldLength;

        return oldLength;
    }
}
