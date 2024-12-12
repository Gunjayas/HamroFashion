using HamroFashion.Api.V1.Exceptions;
using System.Linq.Expressions;
using System.Reflection;

namespace HamroFashion.Api.V1.Extensions
{
    /// <summary>
    /// Some helpers when working with IQueryables
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Allows you to order by the name of a field
        /// </summary>
        /// <typeparam name="T">Derrived from source, the type of queryable thing</typeparam>
        /// <param name="source">The queryable of things</param>
        /// <param name="orderBy">The name of the field to order by</param>
        /// <returns>Source but ordered by field name if possible</returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderBy, bool isDescending = false)
        {
            var type = typeof(T);
            var property = type.GetProperty(orderBy, BindingFlags.IgnoreCase) ??
                type.GetProperty($"{char.ToUpper(orderBy.First())}{orderBy.Substring(1)}");

            if (property == null)
                throw new ApiException("Failed to sort results", new Dictionary<string, string[]>
                {
                    { nameof(orderBy), new [] { $"{orderBy} is not a valid order by field" } },
                    { "sortBy", new [] { $"{orderBy} is not a valid sort by field" } }
                });

            var orderByStatement = isDescending ? "OrderByDescending" : "OrderBy";
            var instance = Expression.Parameter(type, "x");
            var prop = Expression.PropertyOrField(instance, orderBy);
            var lambda = Expression.Lambda(prop, instance);

            var queryStatement = Expression.Call
            (
                typeof(Queryable),
                orderByStatement,
                new Type[] { type, property.PropertyType },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(queryStatement);
        }

        /// <summary>
        /// Helper method to simplify authoring a Where function that has an additional if condition
        /// </summary>
        /// <typeparam name="T">Derrived from source, the type of queryable thing</typeparam>
        /// <param name="source">The queryable of things</param>
        /// <param name="predicate">The where predicate (the x => x.whatever part)</param>
        /// <param name="condition">If true we run the where, if false we do not</param>
        /// <returns>Source but where'd if condition was true</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}
