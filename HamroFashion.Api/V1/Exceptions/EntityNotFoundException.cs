namespace HamroFashion.Api.V1.Exceptions
{
    /// <summary>
    /// Represents a 404 condition
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// An entity model was not found in the database :(
        /// </summary>
        /// <param name="modelName">Should tell the user what model and what id wasn't found</param>
        /// <param name="entityId">Should tell the user what id we were looking for</param>
        /// <param name="innerException">If there was a preceding exception that caused this issue</param>
        public EntityNotFoundException(string modelName, Guid entityId, Exception? innerException = null)
            : base($"{modelName} not found by id '{entityId}'", innerException) { }

        /// <summary>
        /// An entity model was not found in the database :(
        /// </summary>
        /// <param name="modelName">Should tell the user what model and what id wasn't found</param>
        /// <param name="innerException">If there was a preceding exception that caused this issue</param>
        public EntityNotFoundException(string modelName, Exception? innerException = null)
            : base($"{modelName} not found by id 'unknown'", innerException) { }
    }
}
