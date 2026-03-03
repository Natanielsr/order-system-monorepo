using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.API.Security;
using OrderSystem.Application.Authorization;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Users.Commands.Auth;
using OrderSystem.Application.Users.Commands.CreateUser;
using OrderSystem.Application.Users.Commands.GetUser;
using OrderSystem.Application.Users.Commands.UpdateUser;

namespace OrderSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand createUserCommand)
        {
            CreateUserResponseDto response = await mediator.Send(createUserCommand);
            return CreatedAtRoute("GetUserById", new { id = response.Id }, response);
        }

        [Authorize]
        [HttpGet("{id:guid}", Name = "GetUserById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userClaim = APIClaim.createUserClaim(User);
            var authorizationResponse = AuthorizationBase.ValidUser(userClaim, id);
            if (!authorizationResponse.Success)
            {
                return StatusCode(403, authorizationResponse.Message);
            }

            var response = await mediator.Send(new GetUserByIdCommand(id));
            if (response == null)
                return NotFound("User Not Found");

            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand userCommand)
        {
            if (id != userCommand.id)
                return BadRequest("The user ID in the route is different from the ID in the request body.");

            var userClaim = APIClaim.createUserClaim(User);
            var authorizationResponse = AuthorizationBase.ValidUser(userClaim, userCommand.id);
            if (!authorizationResponse.Success)
            {
                return StatusCode(403, authorizationResponse.Message);
            }

            var response = await mediator.Send(userCommand);
            if (response == null)
                return NotFound("User Not Found");

            return Ok(response);
        }




    }
}
