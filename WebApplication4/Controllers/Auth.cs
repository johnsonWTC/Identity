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

        public async Task<IActionResult> RegisterAsync ([FromBody]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var register = await 
            }
        }
    }
}
