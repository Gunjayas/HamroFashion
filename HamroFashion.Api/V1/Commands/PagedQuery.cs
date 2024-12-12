namespace HamroFashion.Api.V1.Commands
{
    /// <summary>
    /// PagedQuery represents a get request that can be filtered, sorted
    /// and paginated.
    /// </summary>
    /// <typeparam name="T">The type of filter query command</typeparam>
    public record PagedQuery<T>
    {
        /// <summary>
        /// The filter command (e.g. the GetEntity command that provides filter criteria)
        /// </summary>
        public T FilterCommand { get; set; } = default;

        /// <summary>
        /// The page number being requested
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of records per page that we are asking for
        /// </summary>
        public int PageSize { get; set; } = 60;

        /// <summary>
        /// The name of the field to sort by
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// If sorting, should we sort in descending order?
        /// </summary>
        public bool SortDescending { get; set; } = false;
    }
}
