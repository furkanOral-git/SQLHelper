using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Core.Domain.Entities;
using SQLHelper.Entities;
using SQLHelper.Enums_Structs;

namespace SQLHelper.Builders_Executers
{
    public class DbCommandExecuter<TEntity> : BaseCommandExecuter<TEntity> where TEntity : Entity, new()
    {


        public DbCommandExecuter(string tableName, string[] columnNames, SqlConnection connection) : base(tableName, columnNames, connection)
        {

        }

        public TEntity? GetBy(Expression<Func<TEntity, bool>> predicate, bool isSearchPatternForStrings)
        {
            var command = new DbCommandBuilder(CommandType.ConditionalSelect, this._tableName, this._columnNames);
            command.ConditionParameter = new ConditionParameter<TEntity>(predicate, isSearchPatternForStrings);
            var commandStr = command.BuildForQuery();
            using (SqlCommand cmd = new SqlCommand(commandStr, _connection))
            {
                return ReadData(cmd, this._columnNames);
            }
        }

        public List<TEntity> GetAllBy(Expression<Func<TEntity, bool>>? predicate = null, bool isSearchPatternForStrings = false)
        {
            var commandType = (predicate == null) ? CommandType.Select : CommandType.ConditionalSelect;
            var command = new DbCommandBuilder(commandType, this._tableName, this._columnNames);
            if (predicate != null) command.ConditionParameter = new ConditionParameter<TEntity>(predicate, isSearchPatternForStrings);
            var commandStr = command.BuildForQuery();
            using (SqlCommand cmd = new SqlCommand(commandStr, _connection))
            {
                return ReadDatas(cmd, this._columnNames);
            }

        }
        public void Insert(TEntity entity)
        {
            var command = new DbCommandBuilder(CommandType.Insert, this._tableName, this._columnNames);
            command.Entity = entity;
            var commandStr = command.BuildForRequest();
            using (SqlCommand cmd = new SqlCommand(commandStr, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(int id)
        {
            var command = new DbCommandBuilder(CommandType.Delete, this._tableName, this._columnNames);
            command.Identifier = id;
            var commandStr = command.BuildForRequest();
            using (SqlCommand cmd = new SqlCommand(commandStr, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void Update(TEntity newEntity)
        {
            var command = new DbCommandBuilder(CommandType.Update, this._tableName, this._columnNames);
            command.Entity = newEntity;
            var commandStr = command.BuildForRequest();
            using (SqlCommand cmd = new SqlCommand(commandStr, _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }



    }
}