using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ShuanWebAPI.Dtos;
using ShuanWebAPI.Models;

namespace ShuanWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly AccountBookContext _AccountBookContext;
        private readonly IConfiguration _configuration;

        public LoginController(AccountBookContext todoContext,
            IConfiguration configuration)
        {
            _AccountBookContext = todoContext;
            _configuration = configuration;
        }
        //
        [HttpPost]
        public string login(LoginPost value)
        {
            var user = (from a in _AccountBookContext.User
                        where a.Account == value.Account
                        && a.Password == value.Password
                        select a).SingleOrDefault();

            if (user == null)
            {
                return "帳號密碼錯誤";
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim("FullName", user.Name),
                    new Claim("EmployeeId", user.UserId.ToString())
                };

                var role = from a in _AccountBookContext.Role
                           where a.UserId == user.UserId
                           select a;

                foreach (var temp in role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, temp.Name));
                }

                var authProperties = new AuthenticationProperties
                {
                    // ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(2)
                };



                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return "ok";
            }
        }


        [HttpPost("jwtLogin")]
        public IActionResult jwtLogin(LoginPost value)
        {
            var user = (from a in _AccountBookContext.User
                        where a.Account == value.Account
                        && a.Password == value.Password
                        select a).SingleOrDefault();

            if (user == null)
            {
                return BadRequest("帳號密碼錯誤");
            }
            else
            {

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Account),
                    new Claim("FullName", user.Name),
                    new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                    new Claim("UserId", user.UserId.ToString())
                };

                var role = from a in _AccountBookContext.Role
                           where a.UserId == user.UserId
                           select a;

                foreach (var temp in role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, temp.Name));
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                var jwt = new JwtSecurityToken // JWT預設設定
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                // 輸出token認證給認證成功的使用者
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);


                return Ok(token);
            }
        }





        [HttpDelete]
        public void logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("NoLogin")]
        public string noLogin()
        {
            return "未登入";
        }

        [HttpGet("NoAccess")]
        public string noAccess()
        {
            return "沒有權限";
        }
    }
}
