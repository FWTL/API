using Microsoft.AspNetCore.Builder;

namespace FWTL.Common.Net.Middlewares
{
    public static class ResponseTimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseTimeMiddleware(this IApplicationBuilder app, string source)
        {
            return app.UseMiddleware<ResponseTimeMiddleware>(source);
        }
    }
}