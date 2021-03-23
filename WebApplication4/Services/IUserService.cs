
using Microsoft.AspNetCore.Identity;
using SharedClass;
using System;
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
        private UserManager<IdentityUser> userMananger;
         
        public UserService(UserManager<IdentityUser> userManager)
        {
            userMananger = userManager;
        }
        public Task<UserManangerResponse> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null) 
            {
                throw new NullReferenceException("Register Model is null");
            }

            if(registerViewModel.Password != registerViewModel.ConfirmPassWord)
            {
                return new UserManangerResponse
                {
                    Message = "Comfirm password does not match password",
                }
            }
        }
    }
}
