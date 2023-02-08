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
            string left = "";
            string node = ResolveNode(_arithmeticExp.NodeType);
            string right = "";
            
            if (_arithmeticExp.Left is MemberExpression)
            {
                var memExp = (MemberExpression)_arithmeticExp.Left;
                left = memExp.Member.Name;
            }
            if (_arithmeticExp.Left is ConstantExpression)
            {
                var constExp = (ConstantExpression)_arithmeticExp.Left;
                left = constExp.Value.ToString();
            }
            if (_arithmeticExp.Right is MemberExpression)
            {
                var memExp = (MemberExpression)_arithmeticExp.Right;
                right = memExp.Member.Name;
            }
            if (_arithmeticExp.Right is ConstantExpression)
            {
                var constExp = (ConstantExpression)_arithmeticExp.Right;
                right = constExp.Value.ToString();
            }
            return (left, node, right);
        }
        public override string ToString()
        {
            var constValue = _constExp.Value?.ToString();
            var values = ResolvePredicate();
            return values.Value.leftOfArithmetic + values.Value.nodeOfArithmetic + values.Value.rightOfArithmetic + ConditionalPredicateStructure.ResolveNode(_node) + constValue;
        }
    }
}