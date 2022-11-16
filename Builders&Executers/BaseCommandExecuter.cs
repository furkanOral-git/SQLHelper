using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SQLHelper.Entities;

namespace SQLHelper.Builders_Executers
{
    public abstract class BaseCommandExecuter<TEntity> where TEntity : BaseEntity, new()
    {
        protected string _tableName;
        protected string[] _columnNames;
        protected SqlConnection _connection;

        public BaseCommandExecuter(string tableName, string[] columnNames, SqlConnection connection)
        {
            _tableName = tableName;
            _columnNames = columnNames;
            _connection = connection;
        }
        private bool IsExistColumn(string columnName)
        {
            if (_columnNames.Contains(columnName))
            {
                return true;
            }
            return false;
        }
        private void MapData(SqlDataReader reader, ref TEntity entity)
        {
            for (int i = 0; i < _columnNames.Length; i++)
            {
                var value = reader[_columnNames[i]];
                typeof(TEntity).GetProperty(_columnNames[i]).GetSetMethod().Invoke(entity, new object?[] { value });
            }
        }
        protected TEntity? ReadData(SqlCommand cmd, string[] columnNames)
        {
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var entity = Activator.CreateInstance<TEntity>();
                MapData(reader, ref entity);
                reader.Close();
                return entity;

            }
            return null;
        }
        protected List<TEntity> ReadDatas(SqlCommand cmd, string[] columnNames)
        {
            var reader = cmd.ExecuteReader();
            List<TEntity> results = new();
            while (reader.Read())
            {
                var entity = Activator.CreateInstance<TEntity>();
                MapData(reader, ref entity);
                results.Add(entity);
            }
            reader.Close();
            return results;
        }
        
        
    }
}