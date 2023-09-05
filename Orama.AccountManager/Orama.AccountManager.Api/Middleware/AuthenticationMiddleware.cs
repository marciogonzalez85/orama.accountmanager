using System.Net;

namespace Orama.AccountManager.Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string headerName = "x-api-key";

        public AuthenticationMiddleware(RequestDelegate next,
        ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey(headerName))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await httpContext.Response.WriteAsync("Missing authentication");

                return;
            }

            var requestApiKey = httpContext.Request.Headers[headerName].ToString();

            var secretApiKey = Environment.GetEnvironmentVariable("SECRET_API_KEY");

            var isEqual = string.Equals(requestApiKey, secretApiKey, StringComparison.InvariantCulture);

            if (!isEqual)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await httpContext.Response.WriteAsync("Invalid authentication");

                return;
            }

            await _next(httpContext);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
