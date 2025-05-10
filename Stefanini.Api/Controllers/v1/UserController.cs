using Asp.Versioning;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stefanini.Application.Command.User;
using Stefanini.Application.Models.User;
using Stefanini.Application.Query.User;

namespace Stefanini.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    public readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> Signup([FromBody] UserInputModel model,  CancellationToken cancellationToken)
    {
        var command = new SignupCommand(model.Email, model.Password);
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.Match<IActionResult>(
            user => Created($"user/{user}", user),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                    ErrorType.Validation => BadRequest(error),
                    ErrorType.Conflict =>  Conflict(error),
                };
            }
        );
    }
    
    [HttpPost("sign-in")]
    public async Task<IActionResult> Sigin([FromBody] UserInputModel model,  CancellationToken cancellationToken)
    {
        var query = new SigninQuery(model.Email, model.Password);
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.Match<IActionResult>(
            token => Ok(token),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                };
            }
        );
    }
}