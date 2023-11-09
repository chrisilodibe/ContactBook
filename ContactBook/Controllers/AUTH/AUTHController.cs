using ContactBook.Core.Interfaces;
using ContactBook.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Controllers.AUTH
{
    [Route("api/[controller]")]
    [ApiController]
    public class AUTHController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        
        public AUTHController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model, string role)
        {

             var registerResult = await _authRepository.RegisterUserAsync(model, ModelState, role);

            if (!registerResult)
            {
                return BadRequest(new
                {
                    Message = "Registration failed; check and confirm you entered a valid role"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "user registration successful"
                });
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authRepository.LoginAsync(model);
            if (token == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Credentials"
                });
            }
            return Ok(new
            {
                Token = token
            });
        }
    }
}
