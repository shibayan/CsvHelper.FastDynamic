using System;
using System.Collections.Generic;

namespace CsvHelper.FastDynamic
{
    internal sealed class CsvHeader
    {
        private string[] _fieldNames;
        private readonly Dictionary<string, int> _fieldNameLookup;

        public string[] FieldNames => _fieldNames;

        public CsvHeader(string[] fieldNames)
        {
            _fieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));

            _fieldNameLookup = new Dictionary<string, int>(fieldNames.Length, StringComparer.Ordinal);

            for (int i = fieldNames.Length - 1; i >= 0; i--)
            {
                var name = fieldNames[i];

                if (name != null)
                {
                    _fieldNameLookup[name] = i;
                }
            }
        }

        public int IndexOfName(string name)
        {
            return name != null && _fieldNameLookup.TryGetValue(name, out var result) ? result : -1;
        }

        public int AddField(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (_fieldNameLookup.ContainsKey(name))
            {
                throw new InvalidOperationException($"Field already exists: {name}");
            }

            int oldLength = _fieldNames.Length;

            Array.Resize(ref _fieldNames, oldLength + 1);

            _fieldNames[oldLength] = name;
            _fieldNameLookup[name] = oldLength;

            return oldLength;
        }
    }
}
