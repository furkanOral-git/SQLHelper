using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLHelper.Builders_Executers;
using Core.Domain.Entities;

namespace SQLHelper.TableManagers
{
    public class DbTableOperationManager<TEntity> : ITableOperationManager<TEntity> where TEntity : Entity, new()
    {
        protected SqlDbContext _context;
        protected DataRow _row;
        protected DbCommandExecuter<TEntity> _commandExecuter;
        public Type ConcernType { get; init; }

        public DbTableOperationManager(SqlDbContext context, DataRow row)
        {
            ConcernType = typeof(TEntity);
            _context = context;
            _row = row;
            _commandExecuter = new((string)_row[2], GetColumnNames(), _context.GetConnection());
        }
        public void Insert(TEntity entity)
        {
            _context.Connect();
            _commandExecuter.Insert(entity);
            _context.Disconnect();
        }
        public void InsertAll(List<TEntity> entities)
        {
            _context.Connect();
            foreach (var item in entities)
            {
                _commandExecuter.Insert(item);
            }
            _context.Disconnect();
        }
        public void Delete(int id)
        {
            _context.Connect();
            _commandExecuter.Delete(id);
            _context.Disconnect();
        }
        public void Update(TEntity entity)
        {
            _context.Connect();
            _commandExecuter.Update(entity);
            _context.Disconnect();
        }
        public TEntity GetBy(Expression<Func<TEntity, bool>> predicate, bool isSearchPatternForStrings = false)
        {
            TEntity result;
            _context.Connect();
            result = _commandExecuter.GetBy(predicate, isSearchPatternForStrings);
            _context.Disconnect();
            return result;
        }
        public List<TEntity> GetAllBy(Expression<Func<TEntity, bool>>? predicate = null, bool isSearchPatternForStrings = false)
        {
            List<TEntity> result;

            _context.Connect();

            if (predicate != null)
            {
                result = _commandExecuter.GetAllBy(predicate,isSearchPatternForStrings);
            }
            else
            {
                result = _commandExecuter.GetAllBy();
            }
            _context.Disconnect();
            return result;
        }
        private string?[] GetColumnNames()
        {
            string[] restrictions = new string[] { null, null, $"{(string)_row[2]}", null };
            _context.Connect();
            return _context.GetConnection()
            .GetSchema("Columns", restrictions)
            .AsEnumerable()
            .Select(s => s.Field<String>("Column_Name"))
            .ToArray();
        }

    }
}
