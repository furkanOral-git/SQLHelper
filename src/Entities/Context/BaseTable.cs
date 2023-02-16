using System.Reflection.Emit;
using SQLHelper.Entities.Structs;
using SQLHelper.Repositories;

namespace SQLHelper.Entities.Context
{
    public abstract class BaseTable
    {
        internal SqlHelperContext Context { get; init; }
        internal string TableName { get; init; }
        internal string[] ColumnNames { get; init; }
        

        internal BaseTable(SqlHelperContext context)
        {
            Context = context;
            var data = context.GetTableMetaData(this.GetType());
            TableName = data.tableName;
            ColumnNames = data.columnNames;
        }
        




    }
}