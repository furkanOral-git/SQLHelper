using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
                    return " + ";
                case ExpressionType.Subtract:
                    return " - ";
                case ExpressionType.Divide:
                    return " / ";
                case ExpressionType.Modulo:
                    return " % ";
                case ExpressionType.Multiply:
                    return " * ";
                default:
                    return "";
            }
        }
        private (string? leftOfArithmetic, string nodeOfArithmetic, string? rightOfArithmetic)? ResolvePredicate()
        {
            string left = ResolveExpression(_arithmeticExp.Left);
            string node = ResolveNode(_arithmeticExp.NodeType);
            string right = ResolveExpression(_arithmeticExp.Right);

            return (left, node, right);
        }
        private string ResolveExpression(Expression exp)
        {
            if (exp is MemberExpression)
            {
                var memExp = (MemberExpression)exp;
                return memExp.Member.Name;
            }
            else if (exp is ConstantExpression)
            {
                var constExp = (ConstantExpression)exp;
                return constExp.Value?.ToString() ?? "";
            }
            else if (exp is UnaryExpression)
            {
                var unaryExp = (UnaryExpression)exp;
                return ResolveExpression(unaryExp.Operand);
            }
            else
            {
                return string.Empty;
            }
        }

        public override string ToString()
        {
            var constValue = _constExp.Value?.ToString();
            var values = ResolvePredicate();
            return values.Value.leftOfArithmetic + values.Value.nodeOfArithmetic + values.Value.rightOfArithmetic + ConditionalPredicateStructure.ResolveNode(_node) + constValue;
        }
    }
}