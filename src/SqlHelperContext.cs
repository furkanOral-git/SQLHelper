using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization.Metadata;

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
        internal (string tableName, string[] columnNames, Type[] ctorArgTypes) GetTableMetaData(Type helperTableType)
        {
            var entityType = helperTableType.GetGenericArguments()[0];
            var properties = entityType.GetProperties();
            var propertyNames = properties.Select(p => p.Name).ToArray();
            var tableName = this.GetType().GetProperties().Single(p => p.PropertyType == helperTableType).Name;

            var parameters = helperTableType.GetConstructors()[0].GetParameters();
            bool HasParameters = true;
            if (parameters.Length == 0)
            {
                HasParameters = false;
            }
            var ctorParameterTypes = HasParameters 
            ? parameters.Select(p => p.ParameterType).ToArray() 
            : Array.Empty<Type>();
            
            return (tableName, propertyNames, ctorParameterTypes);
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