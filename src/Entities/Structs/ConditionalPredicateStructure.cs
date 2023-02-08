using System.Linq.Expressions;


namespace SQLHelper.Entities.Structs
{
    internal ref struct ConditionalPredicateStructure
    {
        private readonly MemberExpression _left;
        private readonly ExpressionType _node;
        private readonly ConstantExpression _right;

        public ConditionalPredicateStructure(MemberExpression left, ExpressionType node, ConstantExpression right)
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
                    return " >= ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.NotEqual:
                    return " <> ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.LessThan:
                    return " < ";
                default:
                    return "";

            }
        }
        public override string ToString()
        {
            var values = ResolvePredicate();
            return values.Item1 + values.Item2 + values.Item3;
        }
        public Tuple<string, string, string> ResolvePredicate()
        {
            var node = ResolveNode(_node);
            var value = new ColumnValue(_right.Value);
            return new Tuple<string, string, string>(_left.Member.Name, node, value.ToString());
        }



    }
}