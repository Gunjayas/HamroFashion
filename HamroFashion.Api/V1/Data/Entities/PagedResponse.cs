using HamroFashion.Api.V1.Commands;

namespace HamroFashion.Api.V1.Data.Entities
{
    /// <summary>
    /// Represents a response to a paged query
    /// </summary>
    /// <typeparam name="T">The type of model in our results</typeparam>
    public record PagedResponse<T>
    {
        /// <summary>
        /// The page number being returned
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of records per page that we are returning
        /// </summary>
        public int PageSize { get; set; } = 60;

        /// <summary>
        /// The results being returned
        /// </summary>
        public List<T>? Results { get; set; }

        /// <summary>
        /// The name of the field we sorted by
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// If sorting, was it in descending order?
        /// </summary>
        public bool SortDescending { get; set; } = false;

        /// <summary>
        /// Construct a new PagedResponse from the provided query and results
        /// </summary>
        /// <typeparam name="TCommand">Derrived from the results, the type of model we are returning</typeparam>
        /// <typeparam name="TResult">Derrived from the query, the type of command we are responding to</typeparam>
        /// <param name="query">The paged query we are responding to</param>
        /// <param name="results">The results we are returning</param>
        /// <returns>PagedResponse of TResult results</returns>
        public static PagedResponse<TResult> FromQuery<TCommand, TResult>(PagedQuery<TCommand> query, List<TResult> results)
        {
            return new PagedResponse<TResult>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Results = results,
                SortBy = query.SortBy,
                SortDescending = query.SortDescending,
            };
        }
    }
}
