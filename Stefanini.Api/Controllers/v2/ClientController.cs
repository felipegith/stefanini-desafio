using Asp.Versioning;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stefanini.Application.Command.Client;
using Stefanini.Application.Models.Client;
using Stefanini.Application.Query.Client;

namespace Stefanini.Api.Controllers.v2;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
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
        if(string.IsNullOrEmpty(model.Address))
           return BadRequest(Error.Validation("Model.Address"));
        
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
    /// <summary>
    /// Endpoint responsável por buscar todos os clientes cadastrados
    /// </summary>
    /// <returns>Retorna o resultado da operação</returns>
    [HttpGet("{userId}")]
    public async Task<IActionResult> Clients(Guid userId, CancellationToken cancellationToken)
    {
        var query = new FindAllClientsQuery(userId);
        
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

    /// <summary>
    /// Endpoint responsável por buscar um cliente pelo seu identificador
    /// </summary>
    /// <returns>Retorna o resultado da operação</returns>
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
    /// <summary>
    /// Endpoint responsável por remover um cliente
    /// </summary>
    /// <returns>Retorna o resultado da operação</returns>
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
    /// <summary>
    /// Endpoint responsável por cadastrar atualizar os dados do clinte
    /// </summary>
    /// <returns>Retorna o resultado da operação</returns>
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] ChangeClientInputModel model,  CancellationToken cancellationToken)
    {
        var command = new ChangeClientCommand(model.Id,model.Name, model.Gender, model.Nacionality, model.Naturality, model.Address, model.Email);
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