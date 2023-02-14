using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using SQLHelper.Entities.Context;
using SQLHelper.Factories;

namespace SQLHelper.Repositories
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IDbEntity, new()
    {
        private readonly HelperTable<TEntity> _table;
        private readonly SqlConnection _connection;
        private static BaseRepository<TEntity> _instance;
        private BaseRepository(HelperTable<TEntity> table)
        {
            _table = table;
            _connection = table.Context.GetConnection();
        }
        public static BaseRepository<TEntity> GetRepo(HelperTable<TEntity> table)
        {
            return _instance ??= new BaseRepository<TEntity>(table);
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

        public IList<TEntity>? GetAllBy(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate, _table);
            IList<TEntity>? results = null;

            using (var cmd = new SqlCommand(command, _connection))
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var entity = EntityFactory.CreateEntityWithReader<TEntity>((IDataRecord)reader, _table.ColumnNames);

                    results ??= new List<TEntity>();
                    results.Add(entity);
                }
                reader.Close();
            }
            return results;
        }

        public TEntity? GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate, _table);
            TEntity? entity = null;
            using (var cmd = new SqlCommand(command, _connection))
            {
                var reader = cmd.ExecuteReader();
                entity = EntityFactory.CreateEntityWithReader<TEntity>((IDataRecord)reader, _table.ColumnNames);
            }
            return entity;
        }

        public IList<TEntity>? SearchLike(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateSearchCommand<TEntity>(predicate, _table);
            IList<TEntity>? results = null;

            using (var cmd = new SqlCommand(command, _connection))
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var entity = EntityFactory.CreateEntityWithReader<TEntity>((IDataRecord)reader, _table.ColumnNames);
                    results ??= new List<TEntity>();
                    results.Add(entity);
                }
                reader.Close();
            }
            return results;
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