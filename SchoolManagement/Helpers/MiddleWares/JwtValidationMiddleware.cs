using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SchoolManagement.Interfaces;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SchoolManagement.Helpers.MiddleWares
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtValidationMiddleware> _logger;

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtValidationMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
           // _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var url = context.Request.Path.Value;
            if (url.StartsWith("/api/Authentication/Login") || url.Contains("html") || url.Contains("schoolNotifications") || url.Length < 2 || url.Contains("/api/Authentication/Register")  || url.StartsWith("/swagger") || url.StartsWith("/favicon.ico"))
            {
                await _next(context);
            }
            else
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var principal = await ValidateToken(token);

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
        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                Log.Error("Token validation failed", ex);
                return null;
            }
        }
    }
}
