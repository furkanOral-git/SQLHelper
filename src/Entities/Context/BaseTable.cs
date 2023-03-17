namespace SQLHelper.Entities.Context
{
    public abstract class BaseTable
    {
        internal SqlHelperContext Context { get; init; }
        internal BaseTable(SqlHelperContext context)
        {
            Context = context;
        }
       

    }
}