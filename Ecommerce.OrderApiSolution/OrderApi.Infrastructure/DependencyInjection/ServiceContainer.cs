using Ecommerce.SharedLibrary.DependenceInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.OrderRepositories;



namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastrutureService(this IServiceCollection services, IConfiguration config)
        {
            //Add Database Connectivity
            //Add authentication scheme
            SharedServiceContainer.AddSharedService<OrderDbContext>(services, config, config["MySerilog:FileName"]!);

            //Create Dependency Injection
            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }

        //Register add policy
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register Middleware
            //Global Exception
            //ListenToApigateway Only -> block all outsides calls
            SharedServiceContainer.UseSharedPolices(app);
            return app;
        }
    }
}
