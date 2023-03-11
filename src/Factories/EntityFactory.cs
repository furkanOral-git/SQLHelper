using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using SQLHelper.Entities.Context;

namespace SQLHelper.Factories
{
    // constructor parameters
    internal class EntityFactory
    {

        public static TEntity CreateEntityWithNoParameter<TEntity>(IDataRecord record, string[] propertyNames)
        where TEntity : class, IDbEntity
        {
            var entity = Activator.CreateInstance<TEntity>();

            for (int i = 0; i < propertyNames.Length; i++)
            {
                var value = GetValue(record, propertyNames[i]);
                setPropertyValue(value, propertyNames[i]);
            }


            void setPropertyValue(object? value, string propertyName)
            {
                typeof(TEntity)
                .GetProperty(propertyName)?
                .SetValue(entity, value);
            }

            return entity;
        }
        public static TEntity CreateEntityWithParameters<TEntity>(IDataRecord record, Type[] ctorArgTypes, string[] propertyNames)
        where TEntity : class, IDbEntity
        {
            IList<object?> args = (IList<object?>)Enumerable.Empty<object>();
            int id = -1;
            for (int i = 0; i < propertyNames.Length; i++)
            {
                var value = GetValue(record, propertyNames[i]);
                if (propertyNames[i] == "Id")
                {
                    id = Convert.ToInt32(value);
                    continue;
                }
                args.Add(value);
            }
            var argsArr = args.ToArray();
            orderArgsByTypeOrder(argsArr);
            TEntity? entity = (TEntity?)Activator.CreateInstance(typeof(TEntity), argsArr);
            if (entity is not null) entity.Id = id;

            return entity;

            void orderArgsByTypeOrder(object?[] args)
            {
                var enumerator = args.AsEnumerable().GetEnumerator();

                for (int i = 0; i < ctorArgTypes.Length; i++)
                {
                    var IsSameType = ctorArgTypes[i].Equals(args[i]);
                    if (!IsSameType)
                    {
                        object? temp = null;
                        var index = findIndex(enumerator, ctorArgTypes[i]);
                        //index shouldn't equal to -1;
                        temp = args[i];
                        args[i] = args[index];
                        args[index] = temp;
                    }
                }
            }
            int findIndex(IEnumerator<object?> enumerator, Type type)
            {
                int index = 0;
                bool HasFound = false;
                while (enumerator.MoveNext())
                {
                    if (type.Equals(enumerator.Current))
                    {
                        HasFound = true;
                        break;
                    }
                    index++;
                }
                return HasFound ? index : -1;
            }
        }
        private static object? GetValue(IDataRecord record, string columnName)
        {
            var index = record.GetOrdinal(columnName);
            return record[index];
        }
    }
}