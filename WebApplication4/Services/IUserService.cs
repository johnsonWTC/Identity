﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4
{
    interface IUserService
    {
        Task<UserManangerResponse> RegisterUserAsync(RegisterViewModel registerViewModel);
    }

    public class UserService : IUserService
    {


    }
}
