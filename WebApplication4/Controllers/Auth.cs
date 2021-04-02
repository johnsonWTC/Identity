using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private IUserService _userService;
        private IMailService _mailService;

        public Auth(IUserService userService, IMailService mailService)
        {
            
            _userService = userService;
            _mailService = mailService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync ([FromBody]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var register = await _userService.RegisterUserAsync(registerViewModel);
                if (register.isSuccess)
                {
                    return Ok(register);
                }
                return BadRequest("some properties are imvalid");
            }
            return BadRequest("some properties are imvalid");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var login = await _userService.LoginUserAsync(loginViewModel);
                if (login.isSuccess)
                {
                    await _mailService.SendEmailAsync(loginViewModel.Email, "new login", "new new new");
                    return Ok(login);
                }
                return BadRequest("some properties are imvalid");
            }
            return BadRequest("some properties are imvalid");
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {

        }
    }
}
