using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using SQLHelper.Entities.Context;
using SQLHelper.Factories;
using SQLHelper.Data;

namespace SQLHelper.Repositories
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IDbEntity
    {
        private readonly SqlHelperContext _context;
        private readonly string[] _propertyNames;
        private readonly Type[] _constructorArgTypes;

        private BaseRepository(SqlHelperContext context)
        {
            _context = context;
            _propertyNames = MetaDataProvider.GetEntityProperties<TEntity>();
            _constructorArgTypes = MetaDataProvider.GetEntityArgTypes<TEntity>();
        }
        public static BaseRepository<TEntity> CreateInstance(SqlHelperContext context)
        {
            return new BaseRepository<TEntity>(context);
        }
        public void Insert(TEntity entity)
        {
            var command = StringCommandFactory.CreateInsertCommand<TEntity>(entity);
            System.Console.WriteLine(command);
            using (var cmd = new SqlCommand(command, _context.GetConnection()))
            {
                _context.Connect();
                cmd.ExecuteNonQuery();
                _context.Disconnect();
            }
        }

        public void RemoveBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateRemoveByCommand<TEntity>(predicate);
            using (var cmd = new SqlCommand(command, _context.GetConnection()))
            {
                _context.Connect();
                cmd.ExecuteNonQuery();
                _context.Disconnect();
            }
        }

        public IList<TEntity>? GetAllBy(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate);
            IList<TEntity>? results = null;

            var entityArgTypes = MetaDataProvider.GetEntityArgTypes<TEntity>();
            var columnNames = MetaDataProvider.GetEntityProperties<TEntity>();

            using (var cmd = new SqlCommand(command, _context.GetConnection()))
            {
                _context.Connect();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TEntity? entity;
                    if (_constructorArgTypes.Length == 0)
                    {
                        entity = EntityFactory.CreateEntityWithNoParameter<TEntity>((IDataRecord)reader, _propertyNames);
                    }
                    else
                    {
                        entity = EntityFactory.CreateEntityWithParameters<TEntity>((IDataRecord)reader, _constructorArgTypes, _propertyNames);
                    }
                    results ??= new List<TEntity>();
                    results.Add(entity);
                }
                reader.Close();
                _context.Disconnect();
            }
            return results;
        }

        public TEntity? GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateGetbyCommand<TEntity>(predicate);
            TEntity? entity = null;
            using (var cmd = new SqlCommand(command, _context.GetConnection()))
            {
                _context.Connect();

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    if (_constructorArgTypes.Length == 0)
                    {
                        entity = EntityFactory.CreateEntityWithNoParameter<TEntity>((IDataRecord)reader, _propertyNames);
                    }
                    else
                    {
                        entity = EntityFactory.CreateEntityWithParameters<TEntity>((IDataRecord)reader, _constructorArgTypes, _propertyNames);
                    }
                }
                reader.Close();
                _context.Disconnect();

            }
            return entity;
        }

        public IList<TEntity>? SearchLike(Expression<Func<TEntity, bool>> predicate)
        {
            var command = StringCommandFactory.CreateSearchCommand<TEntity>(predicate);
            IList<TEntity>? results = null;

            using (var cmd = new SqlCommand(command, _context.GetConnection()))
            {
                _context.Connect();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TEntity? entity;
                    if (_constructorArgTypes.Length == 0)
                    {
                        entity = EntityFactory.CreateEntityWithNoParameter<TEntity>((IDataRecord)reader, _propertyNames);
                    }
                    else
                    {
                        entity = EntityFactory.CreateEntityWithParameters<TEntity>((IDataRecord)reader, _constructorArgTypes, _propertyNames);
                    }
                    results ??= new List<TEntity>();
                    results.Add(entity);
                }
                reader.Close();
                _context.Disconnect();
            }
            return results;
        }

        public void Update(TEntity entity)
        {
            var command = StringCommandFactory.CreateUpdateCommand<TEntity>(entity);
            using (var cmd = new SqlCommand(command, _context.GetConnection()))
            {
                _context.Connect();
                cmd.ExecuteNonQuery();
                _context.Disconnect();
            }
        }


    }
}