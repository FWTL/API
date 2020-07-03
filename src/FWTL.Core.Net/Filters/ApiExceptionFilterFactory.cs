using FWTL.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace FWTL.Common.Net.Filters
{
    public class ApiExceptionFilterFactory : IFilterFactory
    {
        private readonly string _source;

        public ApiExceptionFilterFactory(string source)
        {
            _source = source;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ApiExceptionFilter(
                serviceProvider.GetRequiredService<ILogger>(),
                serviceProvider.GetRequiredService<IWebHostEnvironment>(),
                serviceProvider.GetRequiredService<IGuidService>(),
                _source);
        }
    }
}