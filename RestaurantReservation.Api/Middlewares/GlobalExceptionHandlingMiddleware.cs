using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservation.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
  private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

  public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
  {
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, ex.Message);

      await HandleExceptionAsync(context);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context)
  {
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var problem = new ProblemDetails
    {
      Status = context.Response.StatusCode,
      Type = "Internal server error",
      Title = "Server error",
      Detail = "An internal server has occurred"
    };
    
    context.Response.ContentType = "application/json";

    await context.Response.WriteAsJsonAsync(problem);
  }
}