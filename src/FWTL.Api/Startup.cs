using AutoMapper;
using FWTL.Api.Database;
using FWTL.Common.Commands;
using FWTL.Common.Net.Filters;
using FWTL.Common.Queries;
using FWTL.Core.Commands;
using FWTL.Domain.Users;
using FWTL.RabbitMq;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Serilog;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace FWTL.Api
{
    ////dotnet swagger tofile --output api.json C:\Projects\FWTLAuth\src\FWTL.Api\bin\Debug\netcoreapp3.1\FWTL.Api.dll v1

    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            _configuration = configuration.Build();

            if (!hostingEnvironment.IsDevelopment())
            {
                _configuration = configuration.Build();
            }

            if (hostingEnvironment.IsDevelopment())
            {
                configuration.AddUserSecrets<Startup>();
                _configuration = configuration.Build();
            }

            _hostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers(configuration =>
            {
                configuration.Filters.Add(new ApiExceptionFilterFactory(_hostingEnvironment.EnvironmentName));
            });

            //.AddJsonOptions(o =>
            //{
            //    o.JsonSerializerOptions.Con;
            //    o.JsonSerializerOptions.IgnoreNullValues = true;
            //});

            var defaultSettings = new JsonSerializerSettings()
                .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            JsonConvert.DefaultSettings = () => defaultSettings;

            const string format =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {NewLine}{Message:lj}{NewLine}{Exception}";

            services.AddSingleton<ILogger>(b => new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: format)
                .WriteTo.Seq(_configuration["Seq:Url"])
                .CreateLogger());

            services.AddAutoMapper(
                config =>
                {
                    config.AddProfile(new RequestToCommandProfile(typeof(RegisterUser)));
                    config.AddProfile(new RequestToQueryProfile(typeof(RegisterUser)));
                }, typeof(RequestToCommandProfile).Assembly);

            services.AddDbContext<ApiDatabaseContext>();

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApiDatabaseContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.ApiName = "api";
                    options.RequireHttpsMetadata = false;
                });

            IocConfig.RegisterDependencies(services, _hostingEnvironment);

            services.AddMassTransit(x =>
            {
                var commands = typeof(RegisterUser).Assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t))
                    .ToList();

                x.AddConsumers(typeof(CommandConsumer<RegisterUser.Command>));
                //x.AddConsumers(typeof(QueryConsumer<GetMe.Query, GetMe.Result>));

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.ConfigureJsonSerializer(config =>
                    {
                        config.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                        return config;
                    });

                    cfg.ConfigureJsonDeserializer(config =>
                    {
                        config.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                        return config;
                    });

                    var host = cfg.Host(_configuration["RabbitMq:Url"], h =>
                    {
                        h.Username(_configuration["RabbitMq:Username"]);
                        h.Password(_configuration["RabbitMq:Password"]);
                    });

                    cfg.ReceiveEndpoint("commands", ec =>
                    {
                        ec.ConfigureConsumer(context, typeof(CommandConsumer<RegisterUser.Command>));
                    });

                    //cfg.ReceiveEndpoint("queries", ec =>
                    //{
                    //    ec.ConfigureConsumer(context, typeof(QueryConsumer<GetMe.Query, GetMe.Result>));
                    //});
                }));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "FWTL.Api", Version = "v1" });
                c.CustomSchemaIds(x =>
                {
                    int plusIndex = x.FullName.IndexOf("+");
                    int lastIndexOfDot = x.FullName.LastIndexOf(".");
                    int length = 0;

                    if (plusIndex != -1)
                    {
                        length = plusIndex - lastIndexOfDot - 1;
                    }
                    else
                    {
                        length = x.FullName.Length - lastIndexOfDot - 1;
                    }

                    return x.FullName.Substring(lastIndexOfDot + 1, length);
                });
            });
        }

        public void Configure(IApplicationBuilder app, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            app.UseCors(policy =>
            {
                policy = policy.AllowAnyOrigin();
                policy = policy.AllowAnyMethod();
                policy = policy.AllowAnyHeader();
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FWTL.Api");
                c.DisplayRequestDuration();
            });

            Task startBusTask = Task.Run(async () => await ConfigureAsync(app));
            startBusTask.Wait();

            Task seedDatabase = Task.Run(async () => await new SeedData(userManager, roleManager).UpdateAsync());
            seedDatabase.Wait();
        }

        public async Task ConfigureAsync(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var bus = serviceProvider.GetService<IBusControl>();

            await bus.StartAsync();
        }
    }
}