using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SQLHelper.Enums_Structs;

namespace SQLHelper.Builders_Executers
{
    internal static class DbCommandBuilderExtensions
    {
        private static StringBuilder _builder = new();

        private static string DefaultInsert = "INSERT INTO {tableName} ([-columns-]) VALUES ([-values-]);";
        private static string DefaultDelete = "DELETE FROM {tableName} WHERE [-condition-];";
        private static string DefaultUpdate = "UPDATE {tableName} SET [-columnsvalues-] WHERE [-condition-];";
        private static string DefaultSelect = "SELECT * FROM {tableName};";
        private static string ConditionalSelect = "SELECT * FROM {tableName} WHERE [-condition-];";

        internal static void SetOperation(this DbCommandBuilder command)
        {

            switch (command.CommandType)
            {
                case CommandType.Insert:
                    _builder.Append(DefaultInsert);
                    break;
                case CommandType.Update:
                    _builder.Append(DefaultUpdate);
                    break;
                case CommandType.Delete:
                    _builder.Append(DefaultDelete);
                    break;
                case CommandType.Select:
                    _builder.Append(DefaultSelect);
                    break;
                case CommandType.ConditionalSelect:
                    _builder.Append(ConditionalSelect);
                    break;

            }
            _builder.Replace("{tableName}", command.TableName);
        }
        internal static string BuildForRequest(this DbCommandBuilder command)
        {
            var type = command.CommandType;

            if (type == CommandType.Insert)
            {
                command.InsertOperation();
            }
            else if (type == CommandType.Update)
            {
                command.UpdateOperation();
            }
            else if (type == CommandType.Delete)
            {
                command.DeleteOperation();
            }
            return _builder.ToString();
        }
        internal static string BuildForQuery(this DbCommandBuilder command)
        {
            var type = command.CommandType;

            if (type == CommandType.ConditionalSelect)
            {
                command.ConditionalSelectOperation();
            }

            return _builder.ToString();
        }
        internal static string GetColumnValueAsQueryable(this DbCommandBuilder command, string columnName)
        {
            var entity = command.Entity;
            var value = entity.GetType().GetProperty(columnName).GetValue(entity);
            var valueType = value.GetType();
            if (valueType == typeof(string) || valueType == typeof(char))
            {
                return $"'{value}'";
            }
            return $"{value}";
        }
        private static string GetNodeTypeAsQueryable(NodeType node)
        {
            switch (node)
            {
                case NodeType.Equal:
                    return "=";
                case NodeType.NotEqual:
                    return "<>";
                case NodeType.GreaterOrEqual:
                    return ">=";
                case NodeType.Greater:
                    return ">";
                case NodeType.LessThanOrEqual:
                    return "<=";
                case NodeType.Less:
                    return "<";
                case NodeType.Like:
                    return "LIKE";
                default: return "=";
            }
        }
        private static string GetBinderTypeAsQueryable(BinderType binder)
        {
            switch (binder)
            {
                case BinderType.And:
                    return "AND";
                case BinderType.Or:
                    return "OR";
                case BinderType.None:
                    return "";
                default: return "";
            }
        }
        private static void InsertOperation(this DbCommandBuilder command)
        {

            var columnNames = command.ColumnNames;

            for (int i = 0; i < columnNames.Length; i++)
            {
                var columnName = columnNames[i];
                if (columnName == "Id") continue;
                var columnValue = command.GetColumnValueAsQueryable(columnName);

                if (i == columnNames.Length - 1)
                {
                    _builder.Replace("[-columns-]", $"{columnName}");
                    _builder.Replace("[-values-]", $"{columnValue}");
                    break;

                }
                _builder.Replace("[-columns-]", $"{columnName},[-columns-]");
                _builder.Replace("[-values-]", $"{columnValue},[-values-]");

            }
            _builder.Replace("[-columns-]", "");
            _builder.Replace("[-values-]", "");
        }
        private static void UpdateOperation(this DbCommandBuilder command)
        {

            var columnNames = command.ColumnNames;
            var entity = command.Entity;

            for (int i = 0; i < columnNames.Length; i++)
            {
                var columnName = columnNames[i];
                if (columnName == "Id") continue;
                var columnValue = command.GetColumnValueAsQueryable(columnName);

                if (i == columnNames.Length - 1)
                {
                    _builder.Replace("[-columnsvalues-]", $"{columnName}={columnValue}");
                    break;
                }
                _builder.Replace("[-columnsvalues-]", $"{columnName}={columnValue},[-columnsvalues-]");
            }
            _builder.Replace("[-condition-]", $"Id={entity.Id}");
            _builder.Replace("[-columnsvalues-]", "");
        }
        private static void DeleteOperation(this DbCommandBuilder command)
        {
            var entity = command.Entity;
            _builder.Replace("[-condition-]", $"Id={entity.Id}");
        }
        private static void ConditionalSelectOperation(this DbCommandBuilder command)
        {
            var condition = command.ConditionParameter;
            var binderTypesCount = condition.BinderTypes.Count;

            command.InitPredicates();

            if (binderTypesCount > 0)
            {
                command.InitBinders();
            }

        }
        private static void InitBinders(this DbCommandBuilder command)
        {
            var condition = command.ConditionParameter;
            var binderTypes = condition.BinderTypes.ToArray();

            for (int i = 0; i < binderTypes.Length; i++)
            {
                var binder = GetBinderTypeAsQueryable(binderTypes[i]);
                _builder.Replace($"[-binder[{i}]-]", $"{binder}");
            }
        }
        private static void InitPredicates(this DbCommandBuilder command)
        {
            var condition = command.ConditionParameter;
            var predicates = condition.Predicates.ToArray();

            for (int i = 0; i < predicates.Length; i++)
            {
                var predicate = predicates[i];
                string node = GetNodeTypeAsQueryable(predicate.NodeType);

                _builder.Replace("[-condition-]", $"{predicate.Column} {node} {predicate.Value} [-binder[{i}]-] [-condition-]");
            }
            _builder.Replace("[-condition-]", "");
            int lastIndex = predicates.Length - 1;
            _builder.Replace($"[-binder[{lastIndex}]-]", "");
        }






    }
}