using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using SQLHelper.Entities.Context;

namespace SQLHelper.Entities.Structs
{
    public ref struct PredicateBodyStructure<TEntity>
    where TEntity : class, IDbEntity
    {
        private readonly Expression? _left;
        private readonly ExpressionType _nodeType;
        private readonly Expression? _right;

        // exp : p => p.Name == "Bardak" && p.Price == 10000;
        public PredicateBodyStructure(Expression<Func<TEntity, bool>> predicate) : this((BinaryExpression)predicate.Body)
        {

        }
        private PredicateBodyStructure(BinaryExpression binaryExpression)
        {
            _nodeType = binaryExpression.NodeType;
            _left = binaryExpression.Left;
            _right = binaryExpression.Right;
        }


        public void ResolveBody(ref string str)
        {
            if (_left is BinaryExpression)
            {
                var leftBodyStruct = new PredicateBodyStructure<TEntity>((BinaryExpression)_left);
                leftBodyStruct.ResolveBody(ref str);
                str += PredicateStructure.ResolveNode(_nodeType);
            }
            if (_right is BinaryExpression)
            {
                var rightBodyStruct = new PredicateBodyStructure<TEntity>((BinaryExpression)_right);
                rightBodyStruct.ResolveBody(ref str);
            }
            if (_left is MemberExpression && _right is ConstantExpression)
            {
                var predicateStructure = new PredicateStructure((MemberExpression)_left, _nodeType, (ConstantExpression)_right);
                str += predicateStructure.ToString();
            }

        }



    }
}