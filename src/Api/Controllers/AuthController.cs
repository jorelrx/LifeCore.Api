using System.Net.Http.Headers;
using LifeOS.Application.Auth.Commands;
using LifeOS.Application.Auth.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeOS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(
    IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    [HttpPost("/api/googleauth/login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Login(CancellationToken cancellationToken)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Missing authorization header",
                Detail = "Send the Google ID token using Authorization: Bearer <id_token>.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var parsedHeader) ||
            !string.Equals(parsedHeader.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(parsedHeader.Parameter))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid authorization header",
                Detail = "Send the Google ID token using Authorization: Bearer <id_token>.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        var command = new GoogleLoginCommand(parsedHeader.Parameter);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }
}
