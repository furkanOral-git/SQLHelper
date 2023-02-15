using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SQLHelper.Entities.Context;

namespace SQLHelper.Factories
{
    internal class EntityFactory
    {
        public static TEntity CreateEntityWithReader<TEntity>(IDataRecord record, string[] propertyNames)
        where TEntity : class, IDbEntity, new()
        {
            var entity = Activator.CreateInstance<TEntity>();

            for (int i = 0; i < propertyNames.Length; i++)
            {
                var value = getValue(propertyNames[i]);
                setPropertyValue(value, propertyNames[i]);
            }
            

            object? getValue(string columnName)
            {
                var index = record.GetOrdinal(columnName);
                return record[index];
            }
            void setPropertyValue(object? value, string propertyName)
            {
                typeof(TEntity)
                .GetProperty(propertyName)?
                .SetValue(entity, value);
            }

            return entity;
        }
    }
}