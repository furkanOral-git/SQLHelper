using System.Collections;
using System.Linq.Expressions;
using SQLHelper.Entities.Context;
using SQLHelper.Entities.Structs;
using SQLHelper.Repositories;

namespace SQLHelper.Entities.Context
{
    public sealed class HelperTable<TEntity> : BaseTable
    where TEntity : class, IDbEntity
    {
        internal BaseRepository<TEntity> Repository { get; init; }

        internal HelperTable(SqlHelperContext context) : base(context)
        {
            TableName = SetTableName();
            ColumnNames = SetColumnNames();
            Repository = new BaseRepository<TEntity>(this);
        }
        //For Test
        public HelperTable(string tableName) : base(tableName)
        {
            
        }
        internal EntityStructure GetEntityStructure(TEntity entity)
        {
            return new EntityStructure(entity);
        }
        internal PredicateBodyStructure<TEntity> GetPredicateBodyStructure(Expression<Func<TEntity, bool>> predicate)
        {
            return new PredicateBodyStructure<TEntity>(predicate);
        }



    }
}