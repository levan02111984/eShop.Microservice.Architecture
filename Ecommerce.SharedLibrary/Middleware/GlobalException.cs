using Ecommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message , statusCode);
                }

                //Forbidden : 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allow to access.";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);
                
                //408 : Request timeout
                if(ex is TaskCanceledException || ex is TimeoutException){
                    title = "Out of time";
                    message = "Request timeout ...try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }


                //default exception
                await ModifyHeader(context, title, message, statusCode);
            }
        }
        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(
                new ProblemDetails(){
                    Detail = message,
                    Status = statusCode,
                    Title = title
                }
            ),CancellationToken.None);
        }
    }
}
