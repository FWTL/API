using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FWTL.Common.Net.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly string _source;

        public ResponseTimeMiddleware(RequestDelegate next, ILogger logger, string source)
        {
            _next = next;
            _logger = logger;
            _source = source;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() =>
            {
                watch.Stop();

                string currentUser = context?.User?.Identity?.Name ?? "Anonymous";
                var route = context.GetRouteData();

                if (route != null)
                {
                    var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
                    _logger.LogInformation(
                        "User {currentUser} executed {route} from {source} total elapsed {responseTimeForCompleteRequest} ms",
                        currentUser,
                        $"{context.Request.Method}/{route.Values["controller"]}/{route.Values["action"]}",
                        _source,
                        responseTimeForCompleteRequest);
                }

                return Task.CompletedTask;
            });

            return _next(context);
        }
    }
}