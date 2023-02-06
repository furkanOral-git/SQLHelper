using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLHelper.Entities.Structs
{
    internal ref struct ColumnValuePairs
    {
        private readonly object _entity;
        private readonly Type _entityType;
        private readonly PropertyInfo[] _properties;
        public ColumnValuePair this[string columnName] => new ColumnValuePair(columnName, _properties.SingleOrDefault(p => p.Name == columnName)?.GetValue(_entity));

        public ColumnValuePairs(object entity)
        {
            _entity = entity;
            _entityType = entity.GetType();
            _properties = _entityType.GetProperties().Where(p => p.PropertyType.IsValueType).ToArray();
        }
        public (string names, string values) GetNamesAndValuesSeparately()
        {
            string Comma(int i, int lenght) => (i == lenght - 1) ? string.Empty : ",";

            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();


            for (int i = 0; i < _properties.Length; i++)
            {
                var pair = this[_properties[i].Name];
                names = names.Append(pair.Name + Comma(i, _properties.Length));
                values = values.Append(pair.Value.ToString() + Comma(i, _properties.Length));
            }
            return (names.ToString(), values.ToString());
        }

        public override string ToString()
        {
            StringBuilder pairs = new StringBuilder();

            for (int i = 0; i < _properties.Length; i++)
            {
                var pair = this[_properties[i].Name];
                pairs = pairs.Append(pair.ToString() + ((i == _properties.Length - 1) ? string.Empty : ","));
            }
            return pairs.ToString();
        }
    }
}