using Intl.Realty.Firm.Models.DataTable;
using LinqKit;
using System.Linq.Expressions;
using System.Reflection;

namespace Intl.Realty.Firm.Utility.Utilities
{
    public static class LinqExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(
                                        this IQueryable<T> query,
                                        string orderByMember,
                                        DtOrderDir ascendingDirection)
        {
            var param = Expression.Parameter(typeof(T), "c");

            var body = orderByMember.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            var queryable = ascendingDirection == DtOrderDir.Asc ?
                (IOrderedQueryable<T>)Queryable.OrderBy(query.AsQueryable(), (dynamic)Expression.Lambda(body, param)) :
                (IOrderedQueryable<T>)Queryable.OrderByDescending(query.AsQueryable(), (dynamic)Expression.Lambda(body, param));

            return queryable;
        }
        public static IQueryable<T> WhereDynamic<T>(
            this IQueryable<T> sourceList, string query)
        {

            if (string.IsNullOrEmpty(query))
            {
                return sourceList;
            }

            try
            {
                var properties = typeof(T).GetProperties()
                    .Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);

                //Expression
                sourceList = sourceList.Where(c =>
                    properties.Any(p => p.GetValue(c) != null && p.GetValue(c).ToString()
                        .Contains(query, StringComparison.InvariantCultureIgnoreCase)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sourceList;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByValues) where TEntity : class
        {
            IQueryable<TEntity> returnValue = null!;

            string orderPair = orderByValues.Trim().Split(',')[0];
            string command = orderPair.ToUpper().Contains("DESC") ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            string propertyName = (orderPair.Split(' ')[0]).Trim();

            System.Reflection.PropertyInfo property;
            MemberExpression propertyAccess;

            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields. 
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0])!;
                propertyAccess = Expression.MakeMemberAccess(parameter, property);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    Type t = property.PropertyType;
                    if (!t.IsConstructedGenericType)
                    {
                        property = t.GetProperty(childProperties[i])!;
                    }
                    else
                    {
                        property = t.GetGenericArguments().First().GetProperty(childProperties[i])!;
                    }

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)!;
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

            source.Expression, Expression.Quote(orderByExpression));

            returnValue = source.Provider.CreateQuery<TEntity>(resultExpression);

            if (orderByValues.Trim().Split(',').Count() > 1)
            {
                // remove first item
                string newSearchForWords = orderByValues.ToString().Remove(0, orderByValues.ToString().IndexOf(',') + 1);
                return source.OrderBy(newSearchForWords);
            }

            return returnValue;
        }
        public static IQueryable<T> FilterByColumn<T>(this IQueryable<T> query, string search, string columnName)
        {
            var properties = typeof(T).GetProperties();

            var predicate = PredicateBuilder.New<T>(false);
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == columnName)
                {

                    predicate = predicate.Or(CreateLike<T>(property, search));
                }

            }
            return query.AsExpandable().Where(predicate);
        }
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string search)
        {
            var properties = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(String));
            var predicate = PredicateBuilder.New<T>(false);
            foreach (PropertyInfo property in properties)
            {
                predicate = predicate.Or(CreateLike<T>(property, search));
            }
            return query.AsExpandable().Where(predicate);
        }

        private static Expression<Func<T, bool>> CreateLike<T>(PropertyInfo prop, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "f");
            var propertyAccess = Expression.MakeMemberAccess(parameter, prop);

            // make sure string is not null
            var notNull = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));

            // convert to lower case
            var toLower = Expression.Call(propertyAccess, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes)!);

            // comparison on lower case
            var like = Expression.Call(toLower, "Contains", null, Expression.Constant(value.ToLower(), typeof(string)));

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(notNull, like), parameter);
        }

        public static object Build<TObject>(this TObject source, string propertyName)
        {
            var propInfo = typeof(TObject).GetProperty(propertyName);

            var parameter = Expression.Parameter(typeof(TObject), "x");

            var property = Expression.Property(parameter, propInfo);

            var delegateType = typeof(Func<,>)
                               .MakeGenericType(typeof(TObject), propInfo.PropertyType);

            var lambda = GetExpressionLambdaMethod()
                            .MakeGenericMethod(delegateType)
                            .Invoke(null, new object[] { property, new[] { parameter } });

            return lambda;
        }

        public static MethodInfo GetExpressionLambdaMethod()
        {
            return typeof(Expression)
                          .GetMethods()
                          .Where(m => m.Name == "Lambda")
                          .Select(m => new
                          {
                              Method = m,
                              Params = m.GetParameters(),
                              Args = m.GetGenericArguments()
                          })
                          .Where(x => x.Params.Length == 2
                                      && x.Args.Length == 1
                                      )
                          .Select(x => x.Method)
                          .First();
        }
        public static IQueryable<T> Like<T>(this IQueryable<T> query, string searchstring, string columnName = null)
        {
            var eParam = Expression.Parameter(typeof(T), "e");
            var isLinq = (query.Provider.GetType().IsGenericType && query.Provider.GetType().GetGenericTypeDefinition() == typeof(EnumerableQuery<>));

            MethodInfo IndexOf, StartsWith, EndsWith, Equals;
            MethodCallExpression mceCurrent, mcePrevious;

            Expression method = string.IsNullOrEmpty(columnName) ? eParam : (Expression)Expression.Property(eParam, columnName);

            var likeParts = searchstring.Split(new char[] { '%' });

            for (int i = 0; i < likeParts.Length; i++)
            {
                if (likeParts[i] == string.Empty) continue; // "%a"

                if (i == 0)
                {
                    if (likeParts.Length == 1) // "a"
                    {
                        Equals = isLinq
                            ? Equals = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) })
                            : Equals = typeof(string).GetMethod("Equals", new[] { typeof(string) });
                        mceCurrent = isLinq
                            ? Expression.Call(method, Equals, new Expression[] { Expression.Constant(likeParts[i], typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase) })
                            : Expression.Call(method, Equals, Expression.Constant(likeParts[i], typeof(string)));
                    }
                    else // "a%" or "a%b"
                    {
                        StartsWith = isLinq
                            ? StartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) })
                            : StartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                        mceCurrent = isLinq
                            ? Expression.Call(method, StartsWith, new Expression[] { Expression.Constant(likeParts[i], typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase) })
                            : Expression.Call(method, StartsWith, Expression.Constant(likeParts[i], typeof(string)));
                    }
                    query = query.Where(Expression.Lambda<Func<T, bool>>(mceCurrent, eParam));
                }
                else if (i == likeParts.Length - 1)  // "a%b" or "%b"
                {
                    EndsWith = isLinq
                        ? EndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) })
                        : EndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    mceCurrent = isLinq
                        ? Expression.Call(method, EndsWith, new Expression[] { Expression.Constant(likeParts[i], typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase) })
                        : Expression.Call(method, EndsWith, Expression.Constant(likeParts[i], typeof(string)));
                    query = query.Where(Expression.Lambda<Func<T, bool>>(mceCurrent, eParam));
                }
                else // "a%b%c"
                {
                    IndexOf = isLinq
                        ? IndexOf = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) })
                        : IndexOf = typeof(string).GetMethod("IndexOf", new[] { typeof(string) });
                    mceCurrent = isLinq
                        ? Expression.Call(method, IndexOf, new Expression[] { Expression.Constant(likeParts[i], typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase) })
                        : Expression.Call(method, IndexOf, Expression.Constant(likeParts[i], typeof(string)));
                    mcePrevious = isLinq
                        ? Expression.Call(method, IndexOf, new Expression[] { Expression.Constant(likeParts[i - 1], typeof(string)), Expression.Constant(StringComparison.OrdinalIgnoreCase) })
                        : Expression.Call(method, IndexOf, Expression.Constant(likeParts[i - 1], typeof(string)));
                    query = query.Where(Expression.Lambda<Func<T, bool>>(Expression.LessThan(mcePrevious, mceCurrent), eParam));
                }
            }

            return query;
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression parameter1 = expr1.Parameters[0];
            var visitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter1);
            var body2WithParam1 = visitor.Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, body2WithParam1), parameter1);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private ParameterExpression _oldParameter;
            private ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (ReferenceEquals(node, _oldParameter))
                    return _newParameter;

                return base.VisitParameter(node);
            }
        }
    }
}
