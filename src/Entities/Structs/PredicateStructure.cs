using System.Linq.Expressions;


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
        // Add (+)
        // Divide (/)
        // Modulo (%)
        // Subtract (-)
        // Multiply (*)
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
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Multiply:
                    return "*";
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