using System.Linq.Expressions;
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
            Eklendi : **Searching için Contains,StartsWith veya EndsWith özelliklerini kullanarak sorgu cümlesi oluşturabilme,

        */
        public void ResolveBody(ref string str)
        {

            if (_left is BinaryExpression && _right is BinaryExpression)
            {
                var leftBodyStruct = new PredicateBodyStructure<TEntity>((BinaryExpression)_left);
                leftBodyStruct.ResolveBody(ref str);
                str += ConditionalPredicateStructure.ResolveNode(_nodeType);

                var rightBodyStruct = new PredicateBodyStructure<TEntity>((BinaryExpression)_right);
                rightBodyStruct.ResolveBody(ref str);
            }
            // Aritmetik işlemler ile karşılaştırma sorgusu için
            if (_left is BinaryExpression && IsArithmeticNode(_left.NodeType) && _right is ConstantExpression)
            {
                var predicateStructure = new ArithmeticPredicateStructure((BinaryExpression)_left, _nodeType, (ConstantExpression)_right);
                str += predicateStructure.ToString();
                return;
            }
            // Karşılaştırma için örneğin; eşit mi değil mi sorgusu için.
            if (_left is MemberExpression && _right is ConstantExpression)
            {
                var predicateStructure = new ConditionalPredicateStructure((MemberExpression)_left, _nodeType, (ConstantExpression)_right);
                str += predicateStructure.ToString();
                return;
            }
            // Contains, StartsWith ve EndsWith metodları ile pattern arama için
            if (_left is MethodCallExpression && _right is ConstantExpression)
            {
                var patternStructure = new PatternStructure((MethodCallExpression)_left, (ConstantExpression)_right);
                str += patternStructure.ToString();
                return;
            }

        }
        private bool IsArithmeticNode(ExpressionType nodeType)
        {
            return
            (
                nodeType is ExpressionType.Add
            || nodeType is ExpressionType.Divide
            || nodeType is ExpressionType.Multiply
            || nodeType is ExpressionType.Subtract
            || nodeType is ExpressionType.Modulo
            ) ? true : false;
        }



    }
}