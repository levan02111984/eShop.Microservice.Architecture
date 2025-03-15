using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Processing header
            var singedHeader = context.Request.Headers["Api-gateway"];


            // NUll means, the request is not coming from the Api Gateway // 503 service
            if(singedHeader.FirstOrDefault() == null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service is unavailable");
            }
            else
            {
                await next(context);
            }

        }
    }
}
