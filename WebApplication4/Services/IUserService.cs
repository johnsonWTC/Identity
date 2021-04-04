
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
using WebApplication4.Services;

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
        private IMailService _mailService;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configaration, IMailService mailService)
        {
            userMananger = userManager;
            _configaration = configaration;
            _mailService = mailService;
        }

        public  async Task<UserManangerResponse> CorfirmEmailAsync(string userID, string tocken)
        {
            var user = await userMananger.FindByIdAsync(userID);
            if(user == null)
            {
                return new UserManangerResponse
                {
                    isSuccess = false,
                    Message = "user not found",
                };
            }
            var decodedToken = WebEncoders.Base64UrlDecode(tocken);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

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
                var confirmEmailtoken = await userMananger.GenerateEmailConfirmationTokenAsync(identityUser);
                var ecodedEmailtoken = Encoding.UTF8.GetBytes(confirmEmailtoken);
                var validEmailToken = WebEncoders.Base64UrlEncode(ecodedEmailtoken);
                
                string url = $"{_configaration["AppURL"]}/api/auth/confirmEmail?userid={identityUser.Id}&token={validEmailToken}";

            

                await _mailService.SendEmailAsync(registerViewModel.Email, "new login",$"<a>{url}</a>");


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
