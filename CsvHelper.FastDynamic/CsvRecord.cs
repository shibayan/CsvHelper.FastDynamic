using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace CsvHelper.FastDynamic
{
    internal sealed class CsvRecord : IDictionary<string, object>, IReadOnlyDictionary<string, object>, IDynamicMetaObjectProvider
    {
        private readonly CsvHeader _header;
        private object[] _values;

        internal CsvRecord(CsvHeader header, object[] values)
        {
            _header = header ?? throw new ArgumentNullException(nameof(header));
            _values = values ?? throw new ArgumentNullException(nameof(values));
        }

        private sealed class DeadValue
        {
            public static readonly DeadValue Default = new DeadValue();
            private DeadValue() { }
        }

        #region IEnumerable<KeyValuePair<string, object>>

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var names = _header.FieldNames;

            for (int i = 0; i < names.Length; i++)
            {
                var value = i < _values.Length ? _values[i] : null;

                if (!(value is DeadValue))
                {
                    yield return new KeyValuePair<string, object>(names[i], value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection<KeyValuePair<string, object>>

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            IDictionary<string, object> dic = this;

            dic.Add(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = DeadValue.Default;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return TryGetValue(item.Key, out var value) && Equals(value, item.Value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            IDictionary<string, object> dic = this;

            return dic.Remove(item.Key);
        }

        int ICollection<KeyValuePair<string, object>>.Count => _values.Count(t => !(t is DeadValue));

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;

        #endregion

        #region IDictionary<string, object>

        void IDictionary<string, object>.Add(string key, object value)
        {
            SetValue(key, value, true);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return ContainsKey(key);
        }

        bool IDictionary<string, object>.Remove(string key) => Remove(_header.IndexOfName(key));

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return TryGetValue(key, out value);
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                TryGetValue(key, out var value);

                return value;
            }

            set => SetValue(key, value, false);
        }

        ICollection<string> IDictionary<string, object>.Keys => this.Select(x => x.Key).ToArray();

        ICollection<object> IDictionary<string, object>.Values => this.Select(x => x.Value).ToArray();

        #endregion

        #region IReadOnlyDictionary<string, object>

        bool IReadOnlyDictionary<string, object>.ContainsKey(string key)
        {
            return ContainsKey(key);
        }

        int IReadOnlyCollection<KeyValuePair<string, object>>.Count => _values.Count(x => !(x is DeadValue));

        object IReadOnlyDictionary<string, object>.this[string key]
        {
            get
            {
                TryGetValue(key, out var value);

                return value;
            }
        }

        bool IReadOnlyDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return TryGetValue(key, out value);
        }

        IEnumerable<string> IReadOnlyDictionary<string, object>.Keys => this.Select(x => x.Key);

        IEnumerable<object> IReadOnlyDictionary<string, object>.Values => this.Select(x => x.Value);

        #endregion

        #region IDynamicMetaObjectProvider

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new CsvRecordMetaObject(parameter, BindingRestrictions.Empty, this);
        }

        #endregion

        #region Internal Methods

        internal bool ContainsKey(string key)
        {
            int index = _header.IndexOfName(key);

            return index >= 0 && index < _values.Length && !(_values[index] is DeadValue);
        }

        internal bool TryGetValue(string key, out object value) => TryGetValue(_header.IndexOfName(key), out value);

        internal bool TryGetValue(int index, out object value)
        {
            if (index < 0)
            {
                value = null;

                return false;
            }

            value = index < _values.Length ? _values[index] : null;

            if (value is DeadValue)
            {
                value = null;

                return false;
            }

            return true;
        }

        internal object SetValue(string name, object value, bool isAdd)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            int index = _header.IndexOfName(name);

            if (index < 0)
            {
                index = _header.AddField(name);
            }
            else if (isAdd && index < _values.Length && !(_values[index] is DeadValue))
            {
                throw new ArgumentException("An item with the same name has already been added", nameof(name));
            }

            return SetValue(index, value);
        }

        internal object SetValue(int index, object value)
        {
            int oldLength = _values.Length;

            if (oldLength <= index)
            {
                Array.Resize(ref _values, _header.FieldNames.Length);

                for (int i = oldLength; i < _values.Length; i++)
                {
                    _values[i] = DeadValue.Default;
                }
            }

            return _values[index] = value;
        }

        internal bool Remove(int index)
        {
            if (index < 0 || index >= _values.Length || _values[index] is DeadValue)
            {
                return false;
            }

            _values[index] = DeadValue.Default;

            return true;
        }

        #endregion
    }
}