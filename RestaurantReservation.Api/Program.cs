using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantReservation.Api.Auth;
using RestaurantReservation.Api.Configurations;
using RestaurantReservation.Api.Interfaces;
using RestaurantReservation.Api.Middlewares;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Error()
  .WriteTo.File("logs/errors.txt", rollingInterval: RollingInterval.Day)
  .CreateLogger();

builder.Host.UseSerilog();

var services = builder.Services;

services.AddEndpointsApiExplorer();

services.AddSwaggerGen(setup =>
{
  setup.SwaggerDoc("v1", new OpenApiInfo{ Title = "Restaurant Reservation API", Version = "v1" });

  var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  
  var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
  
  setup.IncludeXmlComments(xmlCommentsFullPath);

  setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
  });
  
  setup.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });
});

services.AddDbContext<RestaurantReservationDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantReservationCoreDb")));

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

services.AddControllers()
  .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
  .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
  .AddFluentValidation(config =>
  {
    config.ImplicitlyValidateChildProperties = true;
    config.ImplicitlyValidateRootCollectionElements = true;
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
  });;

services.Configure<JwtAuthConfig>(builder.Configuration.GetSection(nameof(JwtAuthConfig)));

services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  var config = builder.Configuration.GetSection(nameof(JwtAuthConfig)).Get<JwtAuthConfig>()!;

  var key = Encoding.UTF8.GetBytes(config.Key);

  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = config.Issuer,
    ValidAudience = config.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ClockSkew = TimeSpan.Zero
  };
});

services.AddAuthorization();

services.AddScoped<GlobalExceptionHandlingMiddleware>();

services.AddApiVersioning(setupAction =>
{
  setupAction.AssumeDefaultVersionWhenUnspecified = true;

  setupAction.DefaultApiVersion = new ApiVersion(1, 0);

  setupAction.ReportApiVersions = true;
});

services.AddScoped<ICustomerRepository, CustomerRepository>()
  .AddScoped<IEmployeeRepository, EmployeeRepository>()
  .AddScoped<IMenuItemRepository, MenuItemRepository>()
  .AddScoped<IOrderRepository, OrderRepository>()
  .AddScoped<IOrderItemRepository, OrderItemRepository>()
  .AddScoped<IReservationRepository, ReservationRepository>()
  .AddScoped<IRestaurantRepository, RestaurantRepository>()
  .AddScoped<ITableRepository, TableRepository>()
  .AddScoped<IPasswordHasher, PasswordHasher>()
  .AddScoped<IUserRepository, UserRepository>()
  .AddScoped<IRoleRepository, RoleRepository>()
  .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();