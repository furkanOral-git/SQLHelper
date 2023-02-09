using SQLHelper.Entities.Structs;
using SQLHelper.Repositories;

namespace SQLHelper.Entities.Context
{
    public abstract class BaseTable
    {
        internal SqlHelperContext Context { get; init; }
        internal string TableName { get; init; }

        internal BaseTable(SqlHelperContext context)
        {
            Context = context;
        }
        protected string SetTableName()
        {
            return default;
        }



    }
}