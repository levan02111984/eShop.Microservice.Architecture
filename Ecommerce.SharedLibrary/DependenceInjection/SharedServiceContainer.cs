using Ecommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce.SharedLibrary.DependenceInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TContext>(IServiceCollection services,IConfiguration config, string filename) where TContext : DbContext
        {
            string sqlConnectionString = config.GetConnectionString("eShopConnection")!;

            //Add Generic Database context
            services.AddDbContext<TContext>(
                option => option.UseSqlServer(sqlConnectionString, sqlServerOption => sqlServerOption.EnableRetryOnFailure())
            );

            //configure serilog loggin
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Debug()
                        .WriteTo.Console()
                        .WriteTo.File(path: $"{filename}-.text",
                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss fff zzz} [{level:u3}] {message:lj} {NewLine} {Exception}",
                            rollingInterval: RollingInterval.Day)
                         .CreateLogger();

            //Add Jwt authentication Scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolices(this IApplicationBuilder app)
        {
            //Use global Exception
            app.UseMiddleware<GlobalException>();

            //Register middleware to block outsiders API calls
            //app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }


    }


}
