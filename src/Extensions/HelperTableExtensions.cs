using System.Linq.Expressions;
using SQLHelper.Entities.Context;

namespace SQLHelper
{
    public static class HelperTableExtensions
    {
        public static void Add<TEntity>(this HelperTable<TEntity> table, TEntity entity)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            table.Repository.Add(entity);
            table.Context.Disconnect();
        }
        public static void Remove<TEntity>(this HelperTable<TEntity> table, int id)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            table.Repository.DeleteBy(e => e.Id == id);
            table.Context.Disconnect();
        }
        public static void RemoveBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            table.Repository.DeleteBy(predicate);
            table.Context.Disconnect();
        }
        public static void Update<TEntity>(this HelperTable<TEntity> table, TEntity entity)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            table.Repository.Update(entity);
            table.Context.Disconnect();
        }
        public static TEntity SelectBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            var result = table.Repository.GetBy(predicate);
            table.Context.Disconnect();
            return result;
        }
        public static IList<TEntity> SelectAllBy<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            var result = table.Repository.GetAllBy(predicate);
            table.Context.Disconnect();
            return result;
        }
        public static IList<TEntity> SearchLike<TEntity>(this HelperTable<TEntity> table, Expression<Func<TEntity, string, bool>> predicate)
        where TEntity : class, IDbEntity
        {
            table.Context.Connect();
            var result = table.Repository.SearchLike(predicate);
            table.Context.Disconnect();
            return result;
        }
    }
}