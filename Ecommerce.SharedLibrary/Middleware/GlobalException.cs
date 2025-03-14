using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate requestDeledate)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Error occurred. Please try again!";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await requestDeledate(context);

                //Too many requests  : 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many reuqest. ";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //UnAuthorized : 401 status code
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You donn't have permission access.";
                    await ModifyHeader(context, title, message , statusCode);
                }

                //Forbidden : 403 status code

            }catch(Exception ex)
            {

            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            throw new NotImplementedException();
        }
    }
}
