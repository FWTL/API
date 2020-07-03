using FluentValidation;
using FWTL.Api.Database;
using FWTL.Common.Credentials;
using FWTL.Common.Services;
using FWTL.Core.Commands;
using FWTL.Core.Events;
using FWTL.Core.Queries;
using FWTL.Core.Services;
using FWTL.Domain.Users;
using FWTL.RabbitMq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using System.Reflection;

namespace FWTL.Api
{
    public class IocConfig
    {
        public static void OverrideWithLocalCredentials(IServiceCollection services)
        {
        }

        public static void RegisterCredentials(IServiceCollection services)
        {
            services.AddSingleton(b =>
            {
                var configuration = b.GetService<IConfiguration>();
                return new ApiDatabaseCredentials(new SqlServerDatabaseCredentials(configuration, "Api"));
            });
        }

        public static void RegisterDependencies(IServiceCollection services, IWebHostEnvironment env)
        {
            var domainAssembly = typeof(RegisterUser).GetTypeInfo().Assembly;

            RegisterCredentials(services);

            if (env.IsDevelopment())
            {
                OverrideWithLocalCredentials(services);
            }

            services.Scan(scan =>
                scan.FromAssemblies(domainAssembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces().WithTransientLifetime()
            );

            services.Scan(scan =>
                scan.FromAssemblies(domainAssembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces().WithScopedLifetime()
            );

            services.Scan(scan =>
                scan.FromAssemblies(domainAssembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces().WithScopedLifetime()
            );

            services.Scan(scan =>
                scan.FromAssemblies(domainAssembly)
                    .AddClasses(filter => filter.Where(implementation => typeof(ICommand).IsAssignableFrom(implementation) && typeof(IRequest).IsAssignableFrom(implementation)))
                    .AsSelf()
                    .WithScopedLifetime()
            );

            services.Scan(scan =>
                scan.FromAssemblies(domainAssembly)
                    .AddClasses(filter => filter.Where(implementation => typeof(IQuery).IsAssignableFrom(implementation) && typeof(IRequest).IsAssignableFrom(implementation)))
                    .AsSelf()
                    .WithScopedLifetime()
            );

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEventFactory, EventFactory>();
            services.AddScoped<ICommandDispatcher, RequestDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<IGuidService, GuidService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IClock>(b => SystemClock.Instance);
            services.AddScoped<IRequestToCommandMapper, RequestToCommandMapper>();
            services.AddScoped<IRequestToQueryMapper, RequestToQueryMapper>();
            services.AddSingleton<ITimeZonesService, TimeZonesService>();
        }
    }
}