﻿using Commands.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;
using Queries.User;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var query = new GetAllUsersQuery();
            var response = await mediator.Send(query);

            return Ok(response);
        }

        // GET: api/Users/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var query = new GetUserByIdQuery() { Id = id };
            var response = await mediator.Send(query);

            return response is null ? NotFound() : Ok(response);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            var command = new UpdateUserCommand()
            {
                Id = id,
                FullName  = user.FullName,
                Email = user.Email,
                Credits = user.Credits
            };

            var response = await mediator.Send(command);

            return response is not null ? NoContent() : NotFound();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var command = new CreateUserCommand()
            {
                FullName = user.FullName,
                Email = user.Email,
                Credits  = user.Credits
            };

            var response = await mediator.Send(command);

            return CreatedAtAction(nameof(GetUser), response);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand()
            {
                Id = id
            };

            var response = await mediator.Send(command);

            return response is not null ? NoContent() : NotFound();
        }
    }
}
