using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderSystem.Domain.Exceptions;

namespace OrderSystem.API.Filters;

public class BadRequestFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BadRequest)
        {
            context.ExceptionHandled = true;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = context.Exception.Message, // Use a mensagem da exceção como detalhe
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }

}
