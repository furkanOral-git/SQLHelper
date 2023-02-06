using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLHelper.Entities.Structs
{
    internal ref struct ColumnValuePair
    {
        private readonly string _columnName;
        private readonly ColumnValue _value;
        public ColumnValue Value => _value;
        public string Name => _columnName;

        public ColumnValuePair(string columnName, object? value)
        {
            _columnName = columnName;
            _value = new ColumnValue(value);
        }
        public override string ToString()
        {
            return string.Format("{0}={1}", _columnName, _value.ToString());
        }
    }
}