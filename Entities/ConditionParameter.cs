using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SQLHelper.Enums_Structs;

namespace SQLHelper.Entities
{
    public class ConditionParameter<TEntity> : ConditionParameter
    where TEntity : Entity, new()
    {
        public ConditionParameter(Expression<Func<TEntity, bool>> predicate, bool isSearchPattern) : base(isSearchPattern)
        {
            InitPredicate(predicate);
        }
        private (string columnName, string value) ResolveDatas(string predicateBody, ref bool isString)
        {
            var regexStr = @"[^=<>!()]+";
            var matches = Regex.Matches(predicateBody, regexStr);
            var columnName = matches[0].Value.Split('.')[1].TrimEnd().TrimStart();
            var rawValue = matches[1].Value.TrimEnd().TrimStart();
            var valueType = GetColumnType(columnName);
            if (valueType == typeof(string) || valueType == typeof(char))
            {

                isString = true;
                rawValue = rawValue.Remove(0, 1).Remove(rawValue.Length - 2, 1);
                string value;
                if (_isSearchPattern)
                {
                    value = $"'{rawValue}%'";

                }
                else
                {
                    value = $"'{rawValue}'";
                }

                return (columnName, value);
            }
            return (columnName, rawValue);
        }
        private string[] ResolvePredicateBody(Expression<Func<TEntity, bool>> predicate)
        {
            List<string> predicateBodies = new();
            var regexStr = @"[^&|()]+";
            var predicates = Regex.Matches(predicate.Body.ToString(), regexStr);

            for (int i = 0; i < predicates.Count; i++)
            {
                var strPredicate = predicates[i].Value.TrimEnd().TrimStart();
                if (strPredicate == "AndAlso" || strPredicate == "OrElse")
                {
                    if (strPredicate == "AndAlso") this.BinderTypes.Add(BinderType.And);
                    if (strPredicate == "OrElse") this.BinderTypes.Add(BinderType.Or);
                    continue;
                }
                predicateBodies.Add(strPredicate);

            }
            return predicateBodies.ToArray();
        }
        private void InitPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            var predicates = ResolvePredicateBody(predicate);
            for (int i = 0; i < predicates.Length; i++)
            {
                var strPredicate = predicates[i];
                CreatePredicate(strPredicate);
            }
        }
        private void CreatePredicate(string strPredicate)
        {
            bool isString = false;
            var items = ResolveDatas(strPredicate, ref isString);
            Predicate pre;
            pre.Column = items.columnName;
            pre.Value = items.value;

            if (_isSearchPattern && isString)
            {
                pre.NodeType = GetNodeType(strPredicate, true);
            }
            else
            {
                pre.NodeType = GetNodeType(strPredicate);
            }
            this.Predicates.Add(pre);
        }
        private Type GetColumnType(string columnName)
        {
            var returnType = typeof(TEntity).GetProperty(columnName).PropertyType;
            return returnType;
        }
    }
    public abstract class ConditionParameter
    {
        public List<Predicate> Predicates { get; init; }
        public List<BinderType> BinderTypes { get; init; }
        protected bool _isSearchPattern;
        public ConditionParameter(bool isSearchPattern)
        {
            Predicates = new();
            BinderTypes = new();
            _isSearchPattern = isSearchPattern;
        }

        protected NodeType GetNodeType(string predicate, bool isSearchPattern = false)
        {
            if (isSearchPattern)
            {
                return NodeType.Like;
            }
            string regex = @"(?:<=)?(?:>=)?(?:==)?(?:!=)?(?:<)?(?:>)?";
            var match = Regex.Match(predicate, regex);
            if (match.Success)
            {
                while (match.Value == "")
                {
                    match = match.NextMatch();
                }
            }
            bool IsEqual = match.Value.ToString().Contains("==");
            bool IsNotEqual = match.Value.ToString().Contains("!=");
            bool IsGreaterThanOrEqual = match.Value.ToString().Contains(">=");
            bool IsGreaterThan = match.Value.ToString().Contains(">");
            bool IsLessThanOrEqual = match.Value.ToString().Contains("<=");
            bool IsLessThan = match.Value.ToString().Contains("<");

            if (IsEqual)
            {
                return NodeType.Equal;
            }
            else if (IsNotEqual)
            {
                return NodeType.NotEqual;
            }
            else if (IsLessThanOrEqual)
            {
                return NodeType.LessThanOrEqual;
            }
            else if (IsLessThan)
            {
                return NodeType.Less;
            }
            else if (IsGreaterThanOrEqual)
            {
                return NodeType.GreaterOrEqual;
            }
            else if (IsGreaterThan)
            {
                return NodeType.Greater;
            }
            else
            {
                throw new Exception("Node type error");
            }

        }

    }

}