using SQLHelper.Entities.Structs;
using SQLHelper.Repositories;

namespace SQLHelper.Entities.Context
{
    public abstract class BaseTable
    {
        private readonly SqlHelperContext _context;
        internal SqlHelperContext Context => _context;
        internal string TableName { get; init; }
        internal IList<string> ColumnNames { get; init; }

        internal BaseTable(SqlHelperContext context)
        {
            _context = context;
        }
        //For Test
        public BaseTable(string tableName)
        {
            TableName = tableName;
        }
        //databasede olan benzer isimdeki tablo bulunur. --regex
        protected string SetTableName()
        {
            return default;
        }
        //tabloda bulunan kolon isimleri karşılaşırılarak bulunur ve getirilir.
        protected IList<string> SetColumnNames()
        {
            return default;
        }
        
    }
}