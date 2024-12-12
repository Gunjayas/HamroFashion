using FluentValidation;
using HamroFashion.Api.V1.Exceptions;
using Serilog;
using System.Reflection;

namespace HamroFashion.Api.V1.Middleware
{
    /// <summary>
    /// Custom filter that performs validation via FluentValidation validators
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidationFilter<T> : IEndpointFilter
        where T : class
    {
        /// <summary>
        /// The validator that will be used to validate the model
        /// </summary>
        private readonly IValidator<T> _validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validator">Validator that will be used by the filter to validate incoming requests</param>
        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }


        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var model = context.GetArgument<T>(0);

            var result = await _validator.ValidateAsync(model, context.HttpContext.RequestAborted);

            Log.Debug("Request {RequestId} validation filter passed: {validationFilter}", context.HttpContext.TraceIdentifier, result.IsValid);

            if (result.IsValid) return await next(context);

            throw new ApiException("Invalid request", result.ToDictionary());
        }
    }

    /// <summary>
    /// Extensions for validation system
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Discovers all implementations of AbstractValidator and registers the implementation
        /// with the DI container using a scoped lifetime.  Equivalent to calling:
        /// services.AddScoped&lt;IValidator&lt;T&gt;, ConcreteValidator&gt;(); 
        /// </summary>
        /// <param name="services">The service collection to add validators to</param>
        /// <returns>The service collection (allows builder pattern)</returns>
        public static IServiceCollection DiscoverValidators(this IServiceCollection services)
        {
            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.BaseType is { IsGenericType: true } && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                .ToList()
                .ForEach(type =>
                {
                    var abstractType = typeof(IValidator<>).MakeGenericType(type.BaseType!.GenericTypeArguments);

                    services.AddScoped(abstractType, type);
                });

            return services;
        }

        /// <summary>
        /// Registers an endpoint filter that will validate <typeparam name="T"></typeparam>
        /// on the incoming request.  If validation fails, problem details 400 - Bad Request
        /// will be returned.  Otherwise, control will be passed to next handler
        /// </summary>
        /// <param name="builder">The route builder</param>
        /// <typeparam name="T">The model type to validate</typeparam>
        /// <returns>The builder</returns>
        public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
            where T : class
            => builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}
