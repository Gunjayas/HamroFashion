namespace HamroFashion.Api.V1.Middleware
{
    /// <summary>
    /// CORS (Cross Orrigin Resource Sharing), this middleware intercepts the OPTIONS requests
    /// from browsers that ask us what domains are allowed to make API calls to us.
    /// </summary>
    public static class CorsFilter
    {
        /// <summary>
        /// Registers this middleware filter with the pipeline
        /// </summary>
        /// <param name="app">The WebApplication to register our middleware with</param>
        /// <returns></returns>
        public static WebApplication UseCorsFilter(this WebApplication app)
        {
            app.Use(RespondCorsAsync);
            return app;
        }

        /// <summary>
        /// Intercepts all "OPTIONS" requests and returns 200 with cors headers defined below
        /// </summary>
        /// <param name="context">The HttpContext that we are processing</param>
        /// <param name="next">The next middleware or handler in the chain</param>
        /// <returns>Task that the pipeline awaits</returns>
        public static async Task RespondCorsAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                await WriteCorsAsync(context);
                await context.Response.WriteAsync("");
            }
            else
            {
                context.Response.OnStarting(WriteCorsAsync, context);
                await next(context);
            }
        }

        /// <summary>
        /// Writes CORS headers allowing anyone anywhere to make api calls to us (for now)
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Task WriteCorsAsync(object state)
        {
            var context = (HttpContext)state;

            context.Response.Headers.Append("Access-Control-Allow-Origin", context.Request.Headers.Origin);
            context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Append("Access-Control-Allow-Methods", string.Join(", ", new[] { context.Request.Method }.Union(context.Request.Headers.AccessControlRequestMethod.Select(v => v))));
            context.Response.Headers.Append("Access-Control-Allow-Headers", string.Join(", ", context.Request.Headers.Select(p => p.Key).Union(context.Request.Headers.AccessControlRequestHeaders.Select(v => v))));

            return Task.CompletedTask;
        }
    }
}
