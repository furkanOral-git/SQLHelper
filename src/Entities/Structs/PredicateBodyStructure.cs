using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using SQLHelper.Entities.Context;

namespace SQLHelper.Entities.Structs
{
    public ref struct PredicateBodyStructure<TEntity>
    where TEntity : class, IDbEntity
    {
        private readonly Expression? _left;
        private readonly ExpressionType _nodeType;
        private readonly Expression? _right;

        public PredicateBodyStructure(Expression<Func<TEntity, bool>> predicate) : this((BinaryExpression)predicate.Body)
        {

        }
        private PredicateBodyStructure(BinaryExpression binaryExpression)
        {
            _nodeType = binaryExpression.NodeType;
            _left = binaryExpression.Left;
            _right = binaryExpression.Right;
        }
        /*
            Eklenilecek yeni özellikler: 

            **Dört işlem kullanılarak sorgu cümlesi oluşturabilme,
            **Searching için Contains,StartsWith veya EndsWith özelliklerini kullanarak sorgu cümlesi oluşturabilme,

        */
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
            // Karşılaştırma için örneğin; eşit mi değil mi sorgusu için.
            if (_left is MemberExpression && _right is ConstantExpression)
            {
                var predicateStructure = new PredicateStructure((MemberExpression)_left, _nodeType, (ConstantExpression)_right);
                str += predicateStructure.ToString();
                return;
            }
            if (_left is MethodCallExpression && _right is ConstantExpression)
            {
                var patternStructure = new PatternStructure((MethodCallExpression)_left, (ConstantExpression)_right);
                str += patternStructure.ToString();
                return;
            }

        }



    }
}