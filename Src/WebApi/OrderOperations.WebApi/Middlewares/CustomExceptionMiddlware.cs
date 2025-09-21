using Microsoft.Extensions.Localization;
using OrderOperations.CustomExceptions.Common;
using OrderOperations.WebApi.DTOs;
using OrderOperations.WebApi.Languages;
using OrderOperations.WebApi.Services;
using System.Diagnostics;
using System.Text.Json;

namespace OrderOperations.WebApi.Middlewares;


public class CustomExceptionMiddlware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<Lang> _localizer;

    public CustomExceptionMiddlware(RequestDelegate next, ILoggerService logger, IStringLocalizer<Lang> localizer)
    {
        _next = next;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task Invoke(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        try
        {
            var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                       ?? context.Connection.RemoteIpAddress?.ToString()
                       ?? "Unknown IP";
            var userName = context.User.Identity?.Name ?? "Anonymous";
            _logger.Write(ConsoleColor.DarkBlue, $"{"[Request]",-10}",
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.DarkYellow, $"{"HTTP",-6}",
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.Cyan, $"{context.Request.Method,-5}",
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.Yellow, context.Request.Path,
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.Green, " Lang = " + context.Request.Headers.AcceptLanguage,
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.DarkBlue, " From : ",
                                 ConsoleColor.Magenta, userName + " / " + clientIp);
            await _next(context);
            watch.Stop();
            _logger.Write(ConsoleColor.DarkBlue, $"{"[Response]",-10}",
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.DarkYellow, $"{"HTTP",-6}",
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.Cyan, $"{context.Request.Method,-5}",
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.Yellow, context.Request.Path,
                                 ConsoleColor.White, $"{"-",3}",
                                 ConsoleColor.White, " responded ",
                                 context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299 ? ConsoleColor.Green : ConsoleColor.Red, context.Response.StatusCode,
                                 ConsoleColor.White, " in ",
                                 ConsoleColor.DarkCyan, watch.Elapsed.Milliseconds, ConsoleColor.White, " ms ",
                                 ConsoleColor.Yellow, context.Items.Count);
        }
        catch (BaseCustomExceptions ex)
        {
            watch.Stop();
            var msg = $"{_localizer[ex.Message].Value}";
            msg += $"{(!string.IsNullOrEmpty(ex._param1) ? "&-&" + (_localizer[ex._param1.Split("*")[0]] + ex._param1.Split("*")[1]) : "")}";
            msg += $"{(!string.IsNullOrEmpty(ex._param2) ? "&-&" + (_localizer[ex._param2.Split("*")[0]] + ex._param2.Split("*")[1]) : "")}";
            msg += $"{(!string.IsNullOrEmpty(ex._param3) ? "&-&" + (_localizer[ex._param3.Split("*")[0]] + ex._param3.Split("*")[1].Replace("\r\n", "-")) : "")}";
            await HandleExceptionAsync(context, ex._statusCode, msg, ex.GetType().Name, watch);
        }
        catch (Exception ex)
        {
            watch.Stop();
            var errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += " - " + ex.InnerException.Message;

            var errType = ex.GetType().Name;
            if (ex.InnerException != null)
                errType += " - " + ex.InnerException.GetType().Name;

            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, errorMessage, errType, watch);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, int statusCode, string message, string typeOfException, Stopwatch watch)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        _logger.Write(
                ConsoleColor.DarkRed, $"{"[Error]",-10}",
                ConsoleColor.White, $"{"-",3}",
                ConsoleColor.DarkYellow, $"{"HTTP",-6}",
                ConsoleColor.White, $"{"-",3}",
                ConsoleColor.Cyan, $"{context.Request.Method,-5}",
                ConsoleColor.White, $"{"-",3}",
                ConsoleColor.Yellow, context.Request.Path,
                ConsoleColor.White, $"{"-",3}",
                ConsoleColor.Red, context.Response.StatusCode,
                ConsoleColor.White, " Error Message ",
                ConsoleColor.Yellow, typeOfException + $"{"-",3}" + message,
                ConsoleColor.White, " in ",
                ConsoleColor.DarkCyan, watch.Elapsed.Milliseconds, ConsoleColor.White, " ms"
            );

        var response = new ResponseDTO($"{_localizer["errorMsg"].Value}: {statusCode}", message, typeOfException);

        var result = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(result);
    }
}
public static class CustomExceptionMiddlwareExtension
{
    public static IApplicationBuilder UseCustomExceptionMiddle(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionMiddlware>();
    }
}
