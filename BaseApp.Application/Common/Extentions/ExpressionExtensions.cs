using System.Linq.Expressions;

namespace BaseApp.Application.Common.Extentions
{
    public sealed class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ParameterReplacer(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
            => _parameter;
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var left = new ParameterReplacer(parameter).Visit(expr1.Body);
            var right = new ParameterReplacer(parameter).Visit(expr2.Body);

            var body = Expression.AndAlso(left!, right!);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> AndAlsoIf<T>(this Expression<Func<T, bool>> expr, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? expr.AndAlso(predicate)
                : expr;
        }

        public static Expression<Func<T, bool>> OrElseIf<T>(this Expression<Func<T, bool>> expr, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (!condition)
                return expr;

            var parameter = Expression.Parameter(typeof(T));

            var left = new ParameterReplacer(parameter).Visit(expr.Body);
            var right = new ParameterReplacer(parameter).Visit(predicate.Body);

            var body = Expression.OrElse(left!, right!);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
