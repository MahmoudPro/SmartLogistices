using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ValidationException = SmartLogistics.Application.Common.Exceptions.ValidationException;

namespace SmartLogistics.Infrastructure.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "حدث خطأ غير متوقع: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        // 1. تحديد الـ Status Code والعناوين الأساسية بناءً على نوع الـ Exception
        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                validationException.Message),

            KeyNotFoundException keyNotFoundException => (
                StatusCodes.Status404NotFound,
                "Not Found",
                keyNotFoundException.Message),

            InvalidOperationException invalidOperationException => (
                StatusCodes.Status422UnprocessableEntity,
                "Business Rule Violation",
                invalidOperationException.Message),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Server Error",
                "حدث خطأ داخلي في الخادم، يرجى المحاولة لاحقاً.")
        };

        context.Response.StatusCode = statusCode;

        // 2. بناء كائن الـ ProblemDetails القياسي
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        // 3. التحقق الذكي والصريح: هل الـ Exception الحالي هو خطأ التحقق (Validation)؟
        // إذا كان كذلك، نقوم بحقن الأخطاء داخل الـ Extensions بدون تضارب في الأنواع
        if (exception is ValidationException valException)
        {
            // Since ProblemDetails does not have Extensions, add errors as a top-level property in the JSON
            var problemDetailsWithErrors = new
            {
                problemDetails.Type,
                problemDetails.Title,
                problemDetails.Status,
                problemDetails.Detail,
                problemDetails.Instance,
                errors = valException.Errors
            };
            var json = JsonSerializer.Serialize(problemDetailsWithErrors);
            await context.Response.WriteAsync(json);
            return;
        }

        // 4. تحويل الكائن بالكامل إلى JSON وإرساله للعميل
        var jsonProblemDetails = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(jsonProblemDetails);
    }
}