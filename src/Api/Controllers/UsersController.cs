using AutoMapper;
using LifeOS.Application.Auth.Dtos;
using LifeOS.Application.Users.Commands;
using LifeOS.Application.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LifeOS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserRequestDto request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }
}