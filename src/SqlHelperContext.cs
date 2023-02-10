using System.Data.SqlClient;

namespace SQLHelper
{
    public abstract class SqlHelperContext : IDisposable
    {
        private readonly SqlConnection _connection;
        internal SqlConnection GetConnection() => _connection;
        private readonly string _connectionString;
        internal string ConnectionString { get => _connectionString; }
        public SqlHelperContext(string connectionString)
        {
            _connection = new SqlConnection();
            _connectionString = connectionString;
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}