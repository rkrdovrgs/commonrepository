using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddUniqueConstraint<T>(this DbContext context, params Expression<Func<T, object>>[] props)
        {
            var columnNames = new List<string>();
            foreach (var p in props)
            {
                var expression = GetMemberInfo(p);
                columnNames.Add(expression.Member.Name);
            }


            var tableName = typeof(T).Name;
            var scriptStr = string.Format(
                "IF OBJECT_ID('UC_{0}_{1}', 'UQ') IS NULL " +
                "ALTER TABLE [{0}] ADD CONSTRAINT UC_{0}_{1} UNIQUE([{2}])",
                tableName,
                string.Join("And", columnNames),
                string.Join("], [", columnNames));

            context.Database.ExecuteSqlCommand(scriptStr);
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
    }
}
