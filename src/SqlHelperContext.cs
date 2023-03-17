using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization.Metadata;
using SQLHelper.Entities.Context;

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
        public SqlHelperContext()
        {
            _connection = new SqlConnection();
        }

        internal void Connect()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
                return;
            }
        }
        internal void Disconnect()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
                return;
            }
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