using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Models;
using AllianzBackEnd.Domain;
using Microsoft.AspNetCore.Mvc;
using AllianzBackEnd.Core.Managers;

namespace AllianzBackEnd.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager _userManager;
        public UserController(UserManager userManager) : base()
        {
            _userManager = userManager;
        }

        [HttpPost("RegisterUser")]
        [Produces(typeof(ApiResponse<User>))]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequest request)
        {
            var user = await _userManager.RegisterUser(request);

            return Ok(user);
        }

        [HttpPost("Login")]
        [Produces(typeof(ApiResponse<User>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.Login(request);

            return Ok(user);
        }
    }
}
