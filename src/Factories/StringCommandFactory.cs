using System.Linq.Expressions;
using System.Text;
using SQLHelper.Entities.Context;
using SQLHelper.Data;

namespace SQLHelper.Factories
{
    internal class StringCommandFactory
    {

        //INSERT INTO {TABLE} ({entity.columnNames}) values (entity.columnValues)
        public static string CreateInsertCommand<TEntity>(TEntity entity)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var entityStructure = RuntimeDataResolver.GetEntityStructure(entity);
            var columnsAndValues = entityStructure.GetColumnValuePairs().GetNamesAndValuesSeparately();

            command = command.AppendFormat
            (
            "INSERT INTO {0} ({1}) VALUES ({2});"
            , MetaDataProvider.GetTableName<TEntity>()
            , columnsAndValues.names
            , columnsAndValues.values
            );
            return command.ToString();
        }
        //UPDATE {TABLE} SET {entity.Column}={entity.Column.Value}, ... WHERE {entity.Column}={entity.Column.Value}
        public static string CreateUpdateCommand<TEntity>(TEntity entity)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var entityStructure = RuntimeDataResolver.GetEntityStructure(entity);
            command = command.AppendFormat
            (
                "UPDATE {0} SET {1} WHERE {2};"
                , MetaDataProvider.GetTableName<TEntity>()
                , entityStructure.GetColumnValuePairs().ToString()
                , entityStructure.GetColumnValuePair("Id").ToString()
            );
            return command.ToString();
        }
        //DELETE FROM {TABLE} WHERE {entity.Column}={entity.Column.Value}
        public static string CreateRemoveByCommand<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var predicateBodyStructure = RuntimeDataResolver.GetPredicateBodyStructure(predicate);
            string conditions = "";
            predicateBodyStructure.ResolveBody(ref conditions);
            command = command.AppendFormat
            (
                "DELETE FROM {0} WHERE {1};"
                , MetaDataProvider.GetTableName<TEntity>()
                , conditions
            );
            return command.ToString();
        }
        //SELECT * FROM WHERE {entity.Column}={entity.Column.Value} ...
        public static string CreateGetbyCommand<TEntity>(Expression<Func<TEntity, bool>>? predicate)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            if (predicate is null)
            {
                command.AppendFormat
                (
                    "SELECT * FROM {0}"
                    , MetaDataProvider.GetTableName<TEntity>()
                );
                return command.ToString();
            }
            var predicateBodyStructure = RuntimeDataResolver.GetPredicateBodyStructure(predicate);
            string conditions = "";
            predicateBodyStructure.ResolveBody(ref conditions);
            command = command.AppendFormat
            (
                "SELECT * FROM {0} WHERE {1}"
                , MetaDataProvider.GetTableName<TEntity>()
                , conditions
            );
            return command.ToString();
        }
        //SELECT * FROM WHERE {entity.Column} LIKE {pattern}%
        public static string CreateSearchCommand<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var predicateBodyStructure = RuntimeDataResolver.GetPredicateBodyStructure(predicate);
            string conditions = "";
            predicateBodyStructure.ResolveBody(ref conditions);
            command = command.AppendFormat
            (
                "SELECT * FROM {0} WHERE {1}"
                , MetaDataProvider.GetTableName<TEntity>()
                , conditions
            );
            return command.ToString();
        }
    }
}