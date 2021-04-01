
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedClass;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication4
{
   public interface IUserService
    {
     public   Task<UserManangerResponse> RegisterUserAsync(RegisterViewModel registerViewModel);
     public   Task<UserManangerResponse> LoginUserAsync(LoginViewModel loginViewModel);
     public   Task<UserManangerResponse> CorfirmEmailAsync(string userID, string tocken);
        
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> userMananger;
        private IConfiguration _configaration;
         
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configaration)
        {
            userMananger = userManager;
            _configaration = configaration;
        }

        public  async Task<UserManangerResponse> CorfirmEmailAsync(string userID, string tocken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserManangerResponse> LoginUserAsync(LoginViewModel loginViewModel)
        {
            var user = await userMananger.FindByEmailAsync(loginViewModel.Email);
            if(user == null)
            {
                return new UserManangerResponse
                {
                    Message = "There is no user with that email adress",
                    isSuccess = false,
                };
            };

            var result = await userMananger.CheckPasswordAsync(user, loginViewModel.Password);
            if (result == false)
            {
                return new UserManangerResponse
                {
                    Message = "Wrong password",
                    isSuccess = false,
                };
            };

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginViewModel.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configaration["AuthSettings:key"]));


            var token = new JwtSecurityToken(
               issuer: _configaration["AuthSettings:Issuer"],
               audience: _configaration["AuthSettings:audience"],
               claims : claims,
               expires :DateTime.Now.AddDays(30),   
               signingCredentials :new SigningCredentials(key, SecurityAlgorithms.HmacSha256) );


            var tokenAsstring = new JwtSecurityTokenHandler().WriteToken(token);


            return new UserManangerResponse
            {
                Message = tokenAsstring,
                isSuccess = true,
                Expiringdate = token.ValidTo,
            };
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
