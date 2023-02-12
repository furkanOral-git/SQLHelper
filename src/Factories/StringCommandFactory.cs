using System.Linq.Expressions;
using System.Text;
using SQLHelper.Entities.Context;

namespace SQLHelper.Factories
{
    internal class StringCommandFactory
    {

        //INSERT INTO {TABLE} ({entity.columnNames}) values (entity.columnValues)
        public static string CreateInsertCommand<TEntity>(TEntity entity, HelperTable<TEntity> table)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var entityStructure = table.GetEntityStructure(entity);
            var columnsAndValues = entityStructure.GetColumnValuePairs().GetNamesAndValuesSeparately();

            command = command.AppendFormat
            (
            "INSERT INTO {0} ({1}) VALUES ({2});"
            , table.TableName
            , columnsAndValues.names
            , columnsAndValues.values
            );
            return command.ToString();
        }
        //UPDATE {TABLE} SET {entity.Column}={entity.Column.Value}, ... WHERE {entity.Column}={entity.Column.Value}
        public static string CreateUpdateCommand<TEntity>(TEntity entity, HelperTable<TEntity> table)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var entityStructure = table.GetEntityStructure(entity);
            command = command.AppendFormat
            (
                "UPDATE {0} SET {1} WHERE {2};"
                , table.TableName
                , entityStructure.GetColumnValuePairs().ToString()
                , entityStructure.GetColumnValuePair("Id").ToString()
            );
            return command.ToString();
        }
        //DELETE FROM {TABLE} WHERE {entity.Column}={entity.Column.Value}
        public static string CreateRemoveByCommand<TEntity>(Expression<Func<TEntity, bool>> predicate, HelperTable<TEntity> table)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var predicateBodyStructure = table.GetPredicateBodyStructure(predicate);
            string conditions = "";
            predicateBodyStructure.ResolveBody(ref conditions);
            command = command.AppendFormat
            (
                "DELETE FROM {0} WHERE {1};"
                , table.TableName
                , conditions
            );
            return command.ToString();
        }
        //SELECT * FROM WHERE {entity.Column}={entity.Column.Value} ...
        public static string CreateGetbyCommand<TEntity>(Expression<Func<TEntity, bool>>? predicate, HelperTable<TEntity> table)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            if (predicate is null)
            {
                command.AppendFormat
                (
                    "SELECT * FROM {0}"
                    , table.TableName
                );
                return command.ToString();
            }
            var predicateBodyStructure = table.GetPredicateBodyStructure(predicate);
            string conditions = "";
            predicateBodyStructure.ResolveBody(ref conditions);
            command = command.AppendFormat
            (
                "SELECT * FROM {0} WHERE {1}"
                , table.TableName
                , conditions
            );
            return command.ToString();
        }
        //SELECT * FROM WHERE {entity.Column} LIKE {pattern}%
        public static string CreateSearchCommand<TEntity>(Expression<Func<TEntity, bool>> predicate, HelperTable<TEntity> table)
        where TEntity : class, IDbEntity
        {
            StringBuilder command = new StringBuilder();
            var predicateBodyStructure = table.GetPredicateBodyStructure(predicate);
            string conditions = "";
            predicateBodyStructure.ResolveBody(ref conditions);
            command = command.AppendFormat
            (
                "SELECT * FROM {0} WHERE {1}"
                , table.TableName
                , conditions
            );
            return command.ToString();
        }
    }
}