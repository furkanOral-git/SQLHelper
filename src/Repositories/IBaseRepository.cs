using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLHelper.Entities.Context;

namespace SQLHelper.Repositories
{
    public interface IBaseRepository<TEntity>
    where TEntity:class,IDbEntity,new()
    {
        public void Insert(TEntity entity);
        public void RemoveBy(Expression<Func<TEntity, bool>> predicate);
        public void Update(TEntity entity);
        public TEntity? GetBy(Expression<Func<TEntity, bool>> predicate);
        public IList<TEntity>? GetAllBy(Expression<Func<TEntity, bool>>? predicate);
        public IList<TEntity>? SearchLike(Expression<Func<TEntity,bool>> predicate);
    }
}