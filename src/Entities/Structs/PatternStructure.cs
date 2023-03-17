using System.Linq.Expressions;


namespace SQLHelper.Entities.Structs
{
    internal ref struct PatternStructure
    {
        private readonly MethodCallExpression _call;
        private readonly ConstantExpression _boolean;
        public PatternStructure(MethodCallExpression call, ConstantExpression boolean)
        {
            _call = call;
            _boolean = boolean;
        }
        public override string ToString()
        {
            string notOrLike(bool b) => (b == false) ? "NOT " : "";
            MemberExpression? memExp = null;
            var values = ResolvePattern();
            if (_call.Object is MemberExpression)
            {
                memExp = (MemberExpression)_call.Object;
            }
            var propertyName = memExp?.Member.Name;
            return propertyName + " " + notOrLike(values.notOrLike) + "LIKE " + ResolvePatternNode(values.methodName, values.pattern);
        }
        private string ResolvePatternNode(string methodName, string? pattern)
        {
            switch (methodName)
            {
                case "Contains":
                    return "'%" + pattern + "%'";
                case "StartsWith":
                    return "'" + pattern + "%'";
                case "EndsWith":
                    return "'%" + pattern + "'";
                default:
                    return string.Empty;
            }
        }
        private (string? pattern, string methodName, bool notOrLike) ResolvePattern()
        {
            var constExp = (ConstantExpression)_call.Arguments[0];
            var stringForSearch = constExp?.Value?.ToString();
            var methodName = _call.Method.Name;
            var notOrLike = Convert.ToBoolean(_boolean.Value);

            return (stringForSearch, methodName, notOrLike);
        }
    }
}