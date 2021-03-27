
using Microsoft.AspNetCore.Identity;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4
{
   public interface IUserService
    {
     public   Task<UserManangerResponse> RegisterUserAsync(RegisterViewModel registerViewModel);
     public   Task<UserManangerResponse> LoginUserAsync(LoginViewModel loginViewModel);
        
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> userMananger;
         
        public UserService(UserManager<IdentityUser> userManager)
        {
            userMananger = userManager;
        }

        public Task<UserManangerResponse> LoginUserAsync(LoginViewModel loginViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<UserManangerResponse> RegisterUserAsync(RegisterViewModel registerViewModel)
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
                    isSuccess = false,
                };
            }

            var identityUser = new IdentityUser();
            identityUser.Email = registerViewModel.Email;
            identityUser.UserName = registerViewModel.Email;

            var result = await userMananger.CreateAsync(identityUser, registerViewModel.Password);

            if (result.Succeeded)
            {
                return new UserManangerResponse
                {
                    Message = "user created successfully",
                    isSuccess = true,
                };
            }

            return new UserManangerResponse
            {
                Message = "user was not created",
                Errors = result.Errors.Select(a => a.Description),
                isSuccess = false,
            };
        }



    }
}
