using Newtonsoft.Json;
using SchoolManagement.Interfaces;
using Serilog;
using System.Net;
using System.Security.Principal;

namespace SchoolManagement.Helpers.MiddleWares
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtValidationMiddleware> _logger;
        private readonly IAuthenticateUser _tokenService;

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtValidationMiddleware> logger, IAuthenticateUser tokenService)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var url = context.Request.Path.Value;
            if (url.Contains("Login") || url.Contains("Register")  || url.StartsWith("/swagger") || url.StartsWith("/favicon.ico"))
            {
                await _next(context);
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var principal = await _tokenService.ValidateToken(token);

                if (principal != null)
                {
                    context.User = principal;
                }
                else
                {
                    _logger.LogWarning("Invalid token.");
                    context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized); //Unauthorized
                    var response = new
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                        ResponseMessage = $"Invalid Authrization Token!"
                    };

                    var jsonResponse = JsonConvert.SerializeObject(response);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResponse);
                    return;
                }
            }
            else
            {
                _logger.LogWarning("No Authorization token supplied.");
                context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized); //Unauthorized
                var response = new
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.Unauthorized),
                    ResponseMessage = $"No Authrization Token Supplied!"
                };

                var jsonResponse = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResponse);
                return;
            }

            await _next(context);
        }
    }
}
