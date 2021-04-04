using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class AuthenticateController : ControllerBase
    {
        #region Fields

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        #endregion

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if(user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var loginTime = model.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: loginTime,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userId = user.Id
            });
        }


        [HttpPut]
        [Route("user/update")]
        [Authorize()]
        public async Task<IActionResult> Update(UpdateUserModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();

            user.UserName = model.UserName;
            user.NormalizedUserName = model.UserName.ToUpper();

            await userManager.UpdateAsync(user);
            await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            return Ok();
        }

    }
}
