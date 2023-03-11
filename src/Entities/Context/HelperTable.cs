using System.Collections;
using System.Linq.Expressions;
using System.Net;
using SQLHelper.Entities.Context;
using SQLHelper.Entities.Structs;
using SQLHelper.Repositories;

namespace SQLHelper.Entities.Context
{
    public sealed class HelperTable<TEntity> : BaseTable
    where TEntity : class, IDbEntity
    {
        internal BaseRepository<TEntity> Repository { get; init; }

        private HelperTable(SqlHelperContext context) : base(context)
        {
            Repository = BaseRepository<TEntity>.CreateInstance(this);
        }
        public static HelperTable<TEntity> CreateInstance(SqlHelperContext context)
        {
            return new HelperTable<TEntity>(context);
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