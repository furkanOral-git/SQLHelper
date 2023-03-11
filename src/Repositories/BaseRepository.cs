using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        private BaseRepository(HelperTable<TEntity> table)
        {
            _table = table;
        }
        public static BaseRepository<TEntity> CreateInstance(HelperTable<TEntity> table)
        {
            return new BaseRepository<TEntity>(table);
        }
        public void Insert(TEntity entity)
        {
            var command = StringCommandFactory.CreateInsertCommand<TEntity>(entity, _table);
            System.Console.WriteLine(command);
            using (var cmd = new SqlCommand(command, _table.Context.GetConnection()))
            {
                _table.Context.Connect();
                cmd.ExecuteNonQuery();
                _table.Context.Disconnect();
            }
        }

        public void RemoveBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateRemoveByCommand<TEntity>(predicate, _table);
            using (var cmd = new SqlCommand(command, _table.Context.GetConnection()))
            {
                _table.Context.Connect();
                cmd.ExecuteNonQuery();
                _table.Context.Disconnect();
            }
        }

        public IList<TEntity>? GetAllBy(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate, _table);
            IList<TEntity>? results = null;

            using (var cmd = new SqlCommand(command, _table.Context.GetConnection()))
            {
                _table.Context.Connect();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TEntity? entity;
                    if (_table.ConstructorArgTypes.Length == 0)
                    {
                        entity = EntityFactory.CreateEntityWithNoParameter<TEntity>((IDataRecord)reader, _table.ColumnNames);
                    }
                    else
                    {
                        entity = EntityFactory.CreateEntityWithParameters<TEntity>((IDataRecord)reader, _table.ConstructorArgTypes, _table.ColumnNames);
                    }
                    results ??= new List<TEntity>();
                    results.Add(entity);
                }
                reader.Close();
                _table.Context.Disconnect();
            }
            return results;
        }

        public TEntity? GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate, _table);
            TEntity? entity = null;
            using (var cmd = new SqlCommand(command, _table.Context.GetConnection()))
            {
                _table.Context.Connect();

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    
                    if (_table.ConstructorArgTypes.Length == 0)
                    {
                        entity = EntityFactory.CreateEntityWithNoParameter<TEntity>((IDataRecord)reader, _table.ColumnNames);
                    }
                    else
                    {
                        entity = EntityFactory.CreateEntityWithParameters<TEntity>((IDataRecord)reader, _table.ConstructorArgTypes, _table.ColumnNames);
                    }
                }
                reader.Close();
                _table.Context.Disconnect();

            }
            return entity;
        }

        public IList<TEntity>? SearchLike(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateSearchCommand<TEntity>(predicate, _table);
            IList<TEntity>? results = null;

            using (var cmd = new SqlCommand(command, _table.Context.GetConnection()))
            {
                _table.Context.Connect();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TEntity? entity;
                    if (_table.ConstructorArgTypes.Length == 0)
                    {
                        entity = EntityFactory.CreateEntityWithNoParameter<TEntity>((IDataRecord)reader, _table.ColumnNames);
                    }
                    else
                    {
                        entity = EntityFactory.CreateEntityWithParameters<TEntity>((IDataRecord)reader, _table.ConstructorArgTypes, _table.ColumnNames);
                    }
                    results ??= new List<TEntity>();
                    results.Add(entity);
                }
                reader.Close();
                _table.Context.Disconnect();
            }
            return results;
        }

        public void Update(TEntity entity)
        {
            var command = StringCommandFactory.CreateUpdateCommand<TEntity>(entity, _table);
            using (var cmd = new SqlCommand(command, _table.Context.GetConnection()))
            {
                _table.Context.Connect();
                cmd.ExecuteNonQuery();
                _table.Context.Disconnect();
            }
        }


    }
}