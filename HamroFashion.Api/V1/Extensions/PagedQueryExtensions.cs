using HamroFashion.Api.V1.Commands;

namespace HamroFashion.Api.V1.Extensions
{
    /// <summary>
    /// Helper methods that save you from having to remember this linq stuff all the time
    /// </summary>
    public static class PagedQueryExtensions
    {
        /// <summary>
        /// Applies the sorting and pagination to the provided source
        /// </summary>
        /// <typeparam name="TEntity">Derrived from the source, the type of enumerable things</typeparam>
        /// <typeparam name="TQuery">Derrifed from the query, the type of filter command we are processing</typeparam>
        /// <param name="source">The enumerable of things to paginate</param>
        /// <param name="query">The PagedQuery to apply</param>
        /// <returns>Paginated (but not filtered) enumerable of TEntity</returns>
        public static IEnumerable<TEntity> ToPage<TEntity, TQuery>(this IEnumerable<TEntity> source, PagedQuery<TQuery> query)
        {
            if (query.SortBy != null)
                source = source.OrderBy(query.SortBy, query.SortDescending);
            else
                source = source.OrderBy("Id", true);

            var page = query.PageNumber > 0 ? query.PageNumber : 1;
            var take = query.PageSize > 0 ? query.PageSize : 60;
            var skip = (page - 1) * take;

            source = source
                .Skip(skip)
                .Take(take);

            return source;
        }

        /// <summary>
        /// Applies the sorting and pagination to the provided source
        /// </summary>
        /// <typeparam name="TEntity">Derrived from the source, the type of queryable things</typeparam>
        /// <typeparam name="TQuery">Derrifed from the query, the type of filter command we are processing</typeparam>
        /// <param name="source">The queryable of things to paginate</param>
        /// <param name="query">The PagedQuery to apply</param>
        /// <returns>Paginated (but not filtered) queryable of TEntity</returns>
        public static IQueryable<TEntity> ToPage<TEntity, TQuery>(this IQueryable<TEntity> source, PagedQuery<TQuery> query)
        {
            if (query.SortBy != null)
                source = source.OrderBy(query.SortBy, query.SortDescending);
            else
                source = source.OrderBy("Id", true);

            var page = query.PageNumber > 0 ? query.PageNumber : 1;
            var take = query.PageSize > 0 ? query.PageSize : 60;
            var skip = (page - 1) * take;

            source = source
                .Skip(skip)
                .Take(take);

            return source;
        }
    }
}
