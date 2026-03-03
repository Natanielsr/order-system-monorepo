using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderSystem.Domain.Exceptions;

namespace OrderSystem.API.Filters;

public class ConflictFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ConflictException)
        {
            context.ExceptionHandled = true;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = context.Exception.Message, // Use a mensagem da exceção como detalhe
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status409Conflict
            };
        }
    }
}
