using System.Data.SqlClient;

namespace SQLHelper
{
    public abstract class SqlHelperContext : IDisposable
    {
        private readonly SqlConnection _connection;
        internal SqlConnection GetConnection() => _connection;
        public string ConnectionString { internal get => ConnectionString; set { ConnectionString = value; } }

        public SqlHelperContext()
        {
            _connection = new SqlConnection();
            this.Configure();
        }
        public virtual void Configure() { }
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