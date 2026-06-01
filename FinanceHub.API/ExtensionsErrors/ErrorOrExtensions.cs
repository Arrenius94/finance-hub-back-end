using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.ExtensionsErrors;

public static class ErrorOrExtensions
{
    public static IActionResult ToActionResult<T>(this ErrorOr<T> result)
    {
        if (!result.IsError)
            return new OkObjectResult(result.Value);

        var firstError = result.Errors.First();
        
        var payload = result.Errors.Select(error => new
        {
            code = error.Code,
            message = error.Description
        }).ToList();

        return firstError.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(payload),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(payload),
            ErrorType.NotFound => new NotFoundObjectResult(payload),
            ErrorType.Conflict => new ConflictObjectResult(payload),
            _ => new ObjectResult(payload)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}