using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using SQLHelper.Entities.Context;
using SQLHelper.Factories;

namespace SQLHelper.Repositories
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IDbEntity
    {
        private readonly HelperTable<TEntity> _table;
        private readonly SqlConnection _connection;
        public BaseRepository(HelperTable<TEntity> table)
        {
            _table = table;
            _connection = table.Context.GetConnection();
        }
        public void Insert(TEntity entity)
        {
            var command = StringCommandFactory.CreateInsertCommand<TEntity>(entity, _table);
            using (var cmd = new SqlCommand(command, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateRemoveByCommand<TEntity>(predicate, _table);
            using (var cmd = new SqlCommand(command, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public IList<TEntity> GetAllBy(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate ?? null, _table);
            using (var cmd = new SqlCommand(command, _connection))
            {
                //read datas
            }
            return default;
        }

        public TEntity GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate, _table);
            using (var cmd = new SqlCommand(command, _connection))
            {
                //read data
            }
            return default;
        }

        public IList<TEntity> SearchLike(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateSearchCommand<TEntity>(predicate, _table);
            using (var cmd = new SqlCommand(command, _connection))
            {
                //read datas
            }
            return default;
        }

        public void Update(TEntity entity)
        {
            var command = StringCommandFactory.CreateUpdateCommand<TEntity>(entity, _table);
            using (var cmd = new SqlCommand(command, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}