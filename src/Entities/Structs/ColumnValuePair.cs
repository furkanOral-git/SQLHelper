using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLHelper.Entities.Structs
{
    internal ref struct ColumnValuePair
    {
        public ColumnValue Value { get; init; }
        public string Name { get; init; }
        public ColumnValuePair(string columnName, object? value)
        {
            Name = columnName;
            Value = new ColumnValue(value);
        }
        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value.ToString());
        }
    }
}