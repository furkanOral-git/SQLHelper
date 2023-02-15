using System;
using System.Collections.Generic;
using System.Data;
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
            // Bug var aşağıdaki kodda
            _properties = _entityType.GetProperties().Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)).ToArray();
        }
        public (string names, string values) GetNamesAndValuesSeparately()
        {

            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();


            for (int i = 0; i < _properties.Length; i++)
            {
                if (_properties[i].Name == "Id") continue;
                var pair = this[_properties[i].Name];
                names = names.Append(pair.Name + Comma(i, _properties.Length));
                values = values.Append(pair.Value.ToString() + Comma(i, _properties.Length));
            }
            return (names.ToString(), values.ToString());
        }
        private string Comma(int i, int lenght) => (i == lenght - 1) ? string.Empty : ",";

        public override string ToString()
        {
            StringBuilder pairs = new StringBuilder();

            for (int i = 0; i < _properties.Length; i++)
            {
                if (_properties[i].Name == "Id") continue;
                var pair = this[_properties[i].Name];
                pairs = pairs.Append(pair.ToString() + Comma(i, _properties.Length));
            }
            return pairs.ToString();
        }
    }
}