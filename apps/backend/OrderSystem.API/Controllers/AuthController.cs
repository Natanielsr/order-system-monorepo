using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.Users.Commands.Auth;
using OrderSystem.Application.Users.Commands.AuthEmail;

namespace OrderSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthCommand login)
        {
            var response = await mediator.Send(login);
            return Ok(response);
        }

        [HttpPost("LoginEmail")]
        public async Task<IActionResult> LoginEmail([FromBody] AuthEmailCommand login)
        {
            var response = await mediator.Send(login);
            return Ok(response);
        }
    }
}
