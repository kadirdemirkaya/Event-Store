using EventSourching.Application.Features.Commands.CreateUser;
using EventSourching.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourching.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProcessController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public UserProcessController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateViewModel userCreateViewModel)
        {
            CreateUserCommandRequest commandRequest = new(userCreateViewModel);
            bool response = await _mediatr.Send(commandRequest);

            return response is true ? Ok() : BadRequest();
        }
    }
}
