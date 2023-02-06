using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading.Tasks;
using SQLHelper.Entities.Structs;

namespace SQLHelper.Entities.Structs
{
    internal ref struct PredicateStructure
    {
        private readonly MemberExpression _left;
        private readonly ExpressionType _node;
        private readonly ConstantExpression _right;

        public PredicateStructure(MemberExpression left, ExpressionType node, ConstantExpression right)
        {
            _left = left;
            _node = node;
            _right = right;
        }
        internal static string ResolveNode(ExpressionType node)
        {
            switch (node)
            {
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.LessThan:
                    return "<";
                default:
                    return "";

            }
        }
        public override string ToString()
        {
            var values = ResolvePredicate();
            var value = new ColumnValue(values.Item3);
            return values.Item1 + values.Item2 + value.ToString();
        }
        public Tuple<string, string, object> ResolvePredicate()
        {
            var node = ResolveNode(_node);
            return new Tuple<string, string, object>(_left.Member.Name, node, _right.Value);
        }



    }
}