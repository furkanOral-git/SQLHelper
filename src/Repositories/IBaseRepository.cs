using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SQLHelper.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void DeleteBy(Expression<Func<TEntity, bool>> predicate);
        public TEntity GetBy(Expression<Func<TEntity, bool>> predicate);
        public IList<TEntity> GetAllBy(Expression<Func<TEntity, bool>>? predicate);
        public IList<TEntity> SearchLike(Expression<Func<TEntity, string, bool>> predicate);
    }
}