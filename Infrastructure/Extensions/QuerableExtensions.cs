using System.Linq.Expressions;
using System.Reflection;
using Utilities.Exceptions;
using Utilities.Results;

namespace Infrastructure.Extensions
{
    public static class QuerableExtensions
    {
        #region ApplyFilter 
        public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> query, string columnName, string filterValue) where TEntity : class
        {
            columnName = columnName.ToLower();
            var actualColumnName = typeof(TEntity).GetProperties().FirstOrDefault(p => p.Name.ToLower() == columnName)?.Name;

            if (actualColumnName == null)
            {
                throw new ArgumentException($"Column '{columnName}' does not exist.");
            }
            var parameterExp = Expression.Parameter(typeof(TEntity), "x");
            var propertyExp = Expression.Property(parameterExp, actualColumnName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(filterValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);
            var finalExpression = Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, parameterExp);

            return query.Where(finalExpression);
        }
        #endregion

        #region ApplySort
        public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, string columnName, bool isDescending=false) where TEntity : class
        {
            columnName = columnName.ToLower();
            var actualColumnName = typeof(TEntity).GetProperties().FirstOrDefault(p => p.Name.ToLower() == columnName)?.Name;

            if (actualColumnName == null)
            {
                throw new ArgumentException($"Column '{columnName}' does not exist.");
            }
            string methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = typeof(TEntity).GetProperty(actualColumnName);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var resultExp = Expression.Call(typeof(Queryable), methodName,
                new Type[] { typeof(TEntity), property.PropertyType },
                query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<TEntity>(resultExp);
        }
        #endregion

        #region ApplySearch
        public static IQueryable<TEntity> ApplySearch<TEntity>(this IQueryable<TEntity> query, string searchColumn, string searchValue) where TEntity : class
        {

            searchColumn = searchColumn.ToLower();
            var actualColumnName = typeof(TEntity).GetProperties().FirstOrDefault(p => p.Name.ToLower() == searchColumn)?.Name;

            if (actualColumnName == null)
            {
                throw new ArgumentException($"Column '{searchColumn}' does not exist.");
            }
            var parameterExp = Expression.Parameter(typeof(TEntity), "x");
            var propertyExp = Expression.Property(parameterExp, actualColumnName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            searchValue = searchValue.ToLower();

            var someValue = Expression.Constant(searchValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            return query.Where(Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, parameterExp));
        }
        #endregion
    }



}
