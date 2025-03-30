using Ecommerce.SharedLibrary.DependenceInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //Add database connectivity
            //Add authentication scheme
            SharedServiceContainer.AddSharedService<ProductDbContext>(services, config, config["MySerilog:FineName"]);

            //Create Dependency
            //Register Respoitory 
            services.AddScoped<IProduct, ProductRepository>();
           
            return services;
        } 

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware
            //GLobal Excpetion 
            SharedServiceContainer.UseSharedPolices(app);
            return app;
        }


    }
}
