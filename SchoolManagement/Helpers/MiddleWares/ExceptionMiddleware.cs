namespace SchoolManagement.Helpers.MiddleWares
{
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SchoolManagement.Helpers.DTOs;
    using Serilog;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // Default status code

            // Set status code based on exception type
            if (exception is UnauthorizedAccessException)
                statusCode = HttpStatusCode.Unauthorized;
            else if (exception is NotImplementedException)
                statusCode = HttpStatusCode.NotImplemented;

            // Log the exception here if needed
            Log.Fatal("An Exception Occured before getting to the controller", exception);

            // Serialize the exception details
            var response = new BaseResponse { ResponseCode = "06", ResponseMessage = "An Error Occured, Please Try Again Later" };
            var payload = JsonSerializer.Serialize(response);

            // Set response content type
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Write the response
            return context.Response.WriteAsync(payload);
        }
    }

}
