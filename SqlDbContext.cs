using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SQLHelper.TableManagers;
using Core.Domain.Entities;

namespace SQLHelper
{
    public class SqlDbContext : IDisposable
    {
        private List<ITableOperationManager> _managers;
        private static SqlDbContext _instance;
        private SqlConnection _connection;

        public SqlDbContext()
        {
            var connectionString = GetConnectionString("Sql");
            _connection = new(connectionString);
            _managers = new List<ITableOperationManager>();

        }
        private string GetConnectionString(string conname)
        {
            return new ConfigurationBuilder().AddJsonFile(Directory.GetCurrentDirectory() + @"\" + "appsettings.json").Build().GetConnectionString(conname);
        }
        internal SqlConnection GetConnection()
        {
            return _connection;
        }

        public DbTableOperationManager<TEntity> Set<TEntity>()
        where TEntity : Entity, new()
        {
            var dataRows = GetDataRows();
            var dataRow = GetMatchingDataRow(dataRows, typeof(TEntity));
            if (_managers.Any(m => m.ConcernType == typeof(TEntity)))
            {
                return (DbTableOperationManager<TEntity>)_managers.SingleOrDefault(m => m.ConcernType == typeof(TEntity));
            }
            var manager = new DbTableOperationManager<TEntity>(_instance, dataRow);
            _managers.Add(manager);
            return manager;
        }
        
        private List<DataRow> GetDataRows()
        {
            List<DataRow> dataRows = new();
            Connect();
            var table = _connection.GetSchema("Tables");
            foreach (DataRow row in table.Rows)
            {
                dataRows.Add(row);
            }
            return dataRows;
        }
        private DataRow GetMatchingDataRow(List<DataRow> dataRows, Type entityType, Type? relationType = null)
        {
            DataRow searchedRow = null;
            var IsNull = relationType == null;
            string pattern;
            foreach (DataRow row in dataRows)
            {
                var tableName = row[2].ToString();
                bool IsMatch;
                if (IsNull)
                {
                    pattern = @$"{entityType.Name}?\w+";
                    IsMatch = Regex.IsMatch(tableName, pattern);
                }
                else
                {
                    pattern = @$"({relationType.Name}\w)?\w?{entityType.Name}\w?_?({relationType.Name}\w?)?";
                    IsMatch = Regex.IsMatch(tableName, pattern);
                }

                if (IsMatch)
                {
                    searchedRow = row;
                    break;
                }
            }

            Disconnect();
            return searchedRow;
        }
        public void Connect()
        {
            if (!IsOpen())
            {
                _connection.Open();
            }
        }
        public void Disconnect()
        {
            if (IsOpen())
            {
                _connection.Close();
            }
        }
        public bool IsOpen()
        {
            if (_connection.State == ConnectionState.Open)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                //managed
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                this._connection.Dispose();
            }
            //unmanaged
        }
    }
}