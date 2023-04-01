using SQLHelper.Entities.Context;
using SQLHelper.Entities.Records;

namespace SQLHelper.Data
{
    internal class MetaDataProvider
    {
        private static IDictionary<Type, MetaDataCollection> _datas = new Dictionary<Type, MetaDataCollection>();
        public static void RegisterTable<TEntity>(Type contextType)
        where TEntity : class, IDbEntity
        {
            var entityType = typeof(TEntity);

            var propertyNames = entityType.GetProperties()
            .Select(p => p.Name)
            .ToArray();

            var constructorArgTypes = entityType.GetConstructors()[0].GetParameters()?
            .Select(p => p.ParameterType)
            .ToArray();

            var tableName = contextType.GetProperties()
            .SingleOrDefault(p => p.PropertyType.IsEquivalentTo(typeof(HelperTable<TEntity>)))?
            .Name;

            var tableType = typeof(HelperTable<TEntity>);
            var collection = new MetaDataCollection(tableName, propertyNames, constructorArgTypes);
            
            if (!_datas.ContainsKey(tableType)) _datas.Add(tableType, collection);

        }
        public static string GetTableName<TEntity>()
        where TEntity : class, IDbEntity
        {
            return GetTableName(typeof(HelperTable<TEntity>));
        }
        public static string GetTableName(Type tableType)
        {
            var collection = _datas[tableType];
            return collection.TableName;
        }
        public static string[] GetEntityProperties<TEntity>()
        where TEntity : class, IDbEntity
        {
            return GetEntityProperties(typeof(HelperTable<TEntity>));
        }
        public static string[] GetEntityProperties(Type tableType)
        {
            var collection = _datas[tableType];
            return collection.TablePropertyNames;
        }
        public static Type[] GetEntityArgTypes<TEntity>()
        where TEntity : class, IDbEntity
        {
            return GetEntityArgTypes(typeof(HelperTable<TEntity>));
        }
        public static Type[] GetEntityArgTypes(Type tableType)
        {
            var collection = _datas[tableType];

            return (collection.EntityConstructorTypes is null)
            ? Array.Empty<Type>()
            : collection.EntityConstructorTypes;
        }

    }
}