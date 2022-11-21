using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace SQLHelper.TableManagers
{
    public interface ITableOperationManager<TEntity> : ITableOperationManager
    where TEntity : Entity, new()
    {
        public void Insert(TEntity entity);
        public void InsertAll(List<TEntity> entities);
        public void Delete(int id);
        public void Update(TEntity entity);
        public TEntity GetBy(Expression<Func<TEntity, bool>> predicate, bool isSearchPatternForStrings);
        public List<TEntity> GetAllBy(Expression<Func<TEntity, bool>>? predicate = null, bool isSearchPatternForStrings = false);
    }
    public interface ITableOperationManager
    {
        public Type ConcernType { get; init; }
    }
}