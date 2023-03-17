using System.Linq.Expressions;
using SQLHelper.Entities.Context;
using SQLHelper.Entities.Structs;

namespace SQLHelper.Data
{
    internal class RuntimeDataResolver
    {
        internal static EntityStructure GetEntityStructure<TEntity>(TEntity entity)
        where TEntity : class, IDbEntity
        {
            return new EntityStructure(entity);
        }
        internal static PredicateBodyStructure<TEntity> GetPredicateBodyStructure<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            return new PredicateBodyStructure<TEntity>(predicate);
        }
    }
}