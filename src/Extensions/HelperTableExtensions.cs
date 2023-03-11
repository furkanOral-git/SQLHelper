using System.Linq.Expressions;
using SQLHelper.Entities.Context;

namespace SQLHelper
{
    public static class HelperTableExtensions
    {
        public static void Insert<TEntity>(this HelperTable<TEntity> table, TEntity entity)
        where TEntity : class, IDbEntity
        {
            table.Repository.Insert(entity);
        }
        public static void Remove<TEntity>(this HelperTable<TEntity> table, int id)
        where TEntity : class, IDbEntity
        {
            table.Repository.RemoveBy(e => e.Id == id);
        }
        public static void RemoveBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            table.Repository.RemoveBy(predicate);
        }
        public static void Update<TEntity>(this HelperTable<TEntity> table, TEntity entity)
        where TEntity : class, IDbEntity
        {
            table.Repository.Update(entity);
        }
        public static TEntity? GetBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            var result = table.Repository.GetBy(predicate);
            return result;
        }
        public static IList<TEntity>? GetAllBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IDbEntity
        {
            var result = table.Repository.GetAllBy(predicate);
            return result;
        }
        public static IList<TEntity>? SearchLike<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            var result = table.Repository.SearchLike(predicate);
            return result;
        }
    }
}