using System.Text.Json;

namespace HamroFashion.Api.V1.Exceptions
{
    /// <summary>
    /// A helper extension that represents a user caused api error, the ExceptionFilter
    /// will translate this to a 400: ValidationProblem
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Dictionary of the field errors to report to the user
        /// </summary>
        public IDictionary<string, string[]> FieldErrors { get; private set; }

        /// <summary>
        /// Basic constructor, calls base Exception(message, innerException) and sets the
        /// .FieldErrors dictionary to a camelCase keyed version of the provided fieldErrors
        /// </summary>
        /// <param name="message">The problem detail message</param>
        /// <param name="fieldErrors">The collection of field errors, will be converted to camelCase keys</param>
        /// <param name="innerException">The exception that lead to this exception</param>
        public ApiException(string message, IDictionary<string, string[]> fieldErrors, Exception? innerException = null) : base(message, innerException)
        {
            FieldErrors = new Dictionary<string, string[]>();

            foreach (var error in fieldErrors)
                FieldErrors.Add(JsonNamingPolicy.CamelCase.ConvertName(error.Key), error.Value);

            FieldErrors.Add("", new string[] { message });
        }
    }
}
