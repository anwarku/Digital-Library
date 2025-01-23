using Backend.DTOs;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LoginUser([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var token = _userService.UserLogin(userLoginDto);
                return Ok(new {token});
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] UserRegisterDto userRegisterDto)
        {
            _userService.UserRegistration(userRegisterDto);

            return Created();
        }
    }
}
