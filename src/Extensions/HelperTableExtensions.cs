using System.Linq.Expressions;
using SQLHelper.Entities.Context;

namespace SQLHelper
{
    public static class HelperTableExtensions
    {
        public static void Insert<TEntity>(this HelperTable<TEntity> table, TEntity entity)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            table.Repository.Insert(entity);
            table.Context.Disconnect();
        }
        public static void Remove<TEntity>(this HelperTable<TEntity> table, int id)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            table.Repository.RemoveBy(e => e.Id == id);
            table.Context.Disconnect();
        }
        public static void RemoveBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            table.Repository.RemoveBy(predicate);
            table.Context.Disconnect();
        }
        public static void Update<TEntity>(this HelperTable<TEntity> table, TEntity entity)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            table.Repository.Update(entity);
            table.Context.Disconnect();
        }
        public static TEntity? GetBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            var result = table.Repository.GetBy(predicate);
            table.Context.Disconnect();
            return result;
        }
        public static IList<TEntity>? GetAllBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            var result = table.Repository.GetAllBy(predicate);
            table.Context.Disconnect();
            return result;
        }
        public static IList<TEntity>? SearchLike<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity, new()
        {
            table.Context.Connect();
            var result = table.Repository.SearchLike(predicate);
            table.Context.Disconnect();
            return result;
        }
    }
}