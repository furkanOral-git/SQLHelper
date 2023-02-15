using System.Data.SqlClient;

namespace SQLHelper
{
    public abstract class SqlHelperContext : IDisposable
    {
        private readonly SqlConnection _connection;
        internal SqlConnection GetConnection() => _connection;
        public string ConnectionString
        {
            internal get
            {
                return _connection.ConnectionString;
            }
            set
            {
                _connection.ConnectionString = value;
            }
        }

        protected SqlHelperContext()
        {
            _connection = new SqlConnection();
        }
        internal void Connect()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
                return;
            }
        }
        internal void Disconnect()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                return;
            }
        }
        internal (string tableName, string[] columnNames) GetTableMetaData(Type helperTableType)
        {
            var entityType = helperTableType.GetGenericArguments()[0];
            var properties = entityType.GetProperties();
            var propertyNames = properties.Select(p => p.Name).ToArray();
            var tableName = this.GetType().GetProperties().Single(p=>p.PropertyType == helperTableType).Name;

            return (tableName, propertyNames);
        }

        public void Dispose()
        {
            if (_connection is not null)
            {
                Disconnect();
                _connection.Dispose();
            }
        }
    }
}