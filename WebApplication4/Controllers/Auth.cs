using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private IUserService _userService;

        public Auth(IUserService userService)
        {
            
            _userService = userService;
        }

        public async Task<IActionResult> RegisterAsync ([FromBody]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var register = await _userService.RegisterUserAsync(registerViewModel);
                if (register.isSuccess)
                {
                    return Ok(register);
                }
            }
        }
    }
}
