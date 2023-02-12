using System.Data.SqlClient;

namespace SQLHelper
{
    public abstract class SqlHelperContext : IDisposable
    {
        private readonly SqlConnection _connection;
        internal SqlConnection GetConnection() => _connection;
        private string _connectionString;
        public string ConnectionString { internal get => _connectionString; set { this._connectionString = value; } }
        
        protected SqlHelperContext()
        {
            _connection = new SqlConnection();
            _connectionString = string.Empty;
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
        internal string GetTableName(Type helperTableType)
        {
            return this.GetType().GetProperties().Single(p => p.PropertyType == helperTableType).Name;
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