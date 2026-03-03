using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderSystem.API.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException fluentException)
        {
            context.ExceptionHandled = true;

            // Extraímos apenas as mensagens de erro em uma lista de strings
            var errors = fluentException.Errors
                .Select(error => error.ErrorMessage)
                .ToList();

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Exception",
                Detail = "One or more validation failures have occurred.",
                Instance = context.HttpContext.Request.Path
            };

            problemDetails.Extensions.Add("errors", errors);

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
