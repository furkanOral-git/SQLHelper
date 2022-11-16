using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SQLHelper.Entities;
using SQLHelper.Enums_Structs;

namespace SQLHelper.Builders_Executers
{
    public class DbCommandBuilder
    {
        public string TableName { get; init; }
        public CommandType CommandType { get; init; }
        public ConditionParameter ConditionParameter { get; internal set; }
        public BaseEntity Entity { get; internal set; }
        public int Identifier { get; set; }
        public string[] ColumnNames { get; init; }

        public DbCommandBuilder(CommandType commandType, string tableName, string[] columnNames)
        {
            CommandType = commandType;
            TableName = tableName;
            ColumnNames = columnNames;
            this.SetOperation();
        }








    }
}