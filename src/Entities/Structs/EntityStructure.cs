using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SQLHelper.Entities.Structs
{
    internal ref struct EntityStructure
    {
        private readonly ColumnValuePairs _pairs;

        public EntityStructure(object entity)
        {
            _pairs = new ColumnValuePairs(entity);
        }
        //Verilen varlığın özellik değerlerini düzenli olarak yan yana ekler ve geri döndürür.
        public ColumnValuePairs GetColumnValuePairs()
        {
            return _pairs;
        }
        public ColumnValuePair GetColumnValuePair(string columnName)
        {
            return _pairs[columnName];
        }


    }
}