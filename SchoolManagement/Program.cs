using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProgramApi.Helpers.ActionFilters;
using ProgramApi.Helpers.AutoMapper;
using SchoolManagement.Helpers.DBContext;
using SchoolManagement.Helpers.MiddleWares;
using SchoolManagement.Helpers.SignalR;
using SchoolManagement.Interfaces;
using SchoolManagement.Repositories;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolManagementApi", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Configure Validation
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(ValidationActionFilter));
    opt.ReturnHttpNotAcceptable = false;
});

// Configure DbContext
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configure Authorization
builder.Services.AddAuthorization();

//Configure Auto MAPPER
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();
builder.Services.AddScoped<IAuthenticateUser, AuthenticateUser>();

// Add SignalR
builder.Services.AddSignalR();

//Add buld time logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

// Use the custom JWT validation middleware
app.UseMiddleware<JwtValidationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<SchoolHub>("/schoolHub");

app.Run();
