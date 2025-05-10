using Asp.Versioning;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Models.Client;
using Stefanini.Application.Query.Client;

namespace Stefanini.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
       _mediator = mediator;
    }
    /// <summary>
    /// Endpoint responsável por cadastrar um cliente   
    /// </summary>
    /// <returns>Retorna o resultado da operação</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientCommandInputModel model, CancellationToken cancellationToken)
    {
        var command = new CreateClientCommand(model);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.Match<IActionResult>(
            client => Created($"client/{client}", client),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                    ErrorType.Validation => BadRequest(error),
                };
            }
        );
    }

    [HttpGet]
    public async Task<IActionResult> Clients(CancellationToken cancellationToken)
    {
        var query = new FindAllClientsQuery();
        
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            client => Ok(client),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                    ErrorType.Validation => BadRequest(error),
                };
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Client(Guid id, CancellationToken cancellationToken)
    {
        var query = new FindClientQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result.Match<IActionResult>(
            client => Ok(client),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                    ErrorType.Validation => BadRequest(error),
                };
            }
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new ClientRemovedCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(
            client => NoContent(),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                    ErrorType.Validation => BadRequest(error),
                };
            }
        );
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] ChangeClientInputModel model,  CancellationToken cancellationToken)
    {
        var command = new ChangeClientCommand(model.Id,model.Name, model.Gender);
        var result = await _mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(
            client => NoContent(),
            errors =>
            {
                var error = errors.FirstOrDefault();
                return error.Type switch
                {
                    ErrorType.Failure => StatusCode(500, error.Description),
                    ErrorType.NotFound => NotFound(error),
                    ErrorType.Validation => BadRequest(error),
                };
            }
        );
    }
}