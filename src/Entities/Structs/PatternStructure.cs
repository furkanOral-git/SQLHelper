using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
            string notOrLike(bool b) => (b == true) ? "NOT" : string.Empty;

            var values = ResolvePattern();

            return notOrLike(values.notOrLike) + " LIKE " + ResolvePatternNode(values.methodName, values.pattern);
        }
        private string ResolvePatternNode(string methodName, string? pattern)
        {
            switch (methodName)
            {
                case "Contains":
                    return "%" + pattern + "%";
                case "StartsWith":
                    return pattern + "%";
                case "EndsWith":
                    return "%" + pattern;
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