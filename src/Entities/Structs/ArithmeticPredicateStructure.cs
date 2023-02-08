using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SQLHelper.Entities.Structs
{
    internal ref struct ArithmeticPredicateStructure
    {
        private readonly BinaryExpression _arithmeticExp;
        private readonly ExpressionType _node;
        private readonly ConstantExpression _constExp;
        public ArithmeticPredicateStructure(BinaryExpression arithmeticExp, ExpressionType node, ConstantExpression constExp)
        {
            _arithmeticExp = arithmeticExp;
            _node = node;
            _constExp = constExp;
        }
        // Add (+)
        // Divide (/)
        // Modulo (%)
        // Subtract (-)
        // Multiply (*)
        internal static string ResolveNode(ExpressionType expType)
        {
            switch (expType)
            {
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
        private (string leftOfArithmetic, string nodeOfArithmetic, string? rightOfArithmetic)? ResolvePredicate()
        {
            if (_arithmeticExp.Left is MemberExpression && _arithmeticExp.Right is ConstantExpression)
            {
                var memExp = (MemberExpression)_arithmeticExp.Left;
                var constExp = (ConstantExpression)_arithmeticExp.Right;

                var methodName = memExp.Member.Name;
                var node = ResolveNode(_arithmeticExp.NodeType);
                var rightConst = constExp.Value?.ToString();

                return (methodName, node, rightConst);
            }
            if (_arithmeticExp.Left is MemberExpression && _arithmeticExp.Right is MemberExpression)
            {
                var memExp = (MemberExpression)_arithmeticExp.Left;
                var rightMemExp = (MemberExpression)_arithmeticExp.Right;

                var methodName = memExp.Member.Name;
                var node = ResolveNode(_arithmeticExp.NodeType);
                var rightMethodName = rightMemExp.Member.Name;

                return (methodName, node, rightMethodName);
            }
            return null;
        }
        public override string ToString()
        {
            var constValue = _constExp.Value?.ToString();
            var values = ResolvePredicate();
            return values.Value.leftOfArithmetic + values.Value.nodeOfArithmetic + values.Value.rightOfArithmetic + ConditionalPredicateStructure.ResolveNode(_node) + constValue;
        }
    }
}