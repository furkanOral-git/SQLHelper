using SQLHelper.Data;
using SQLHelper.Repositories;

namespace SQLHelper.Entities.Context
{
    public sealed class HelperTable<TEntity> : BaseTable
    where TEntity : class, IDbEntity
    {
        internal BaseRepository<TEntity> Repository { get; init; }

        private HelperTable(SqlHelperContext context) : base(context)
        {
            Repository = BaseRepository<TEntity>.CreateInstance(context);
        }
        public static HelperTable<TEntity> CreateInstance(SqlHelperContext context)
        {
            var table = new HelperTable<TEntity>(context);
            MetaDataProvider.RegisterTable<TEntity>(context.GetType());
            return table;
        }
        

    }
}