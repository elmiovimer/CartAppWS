using CartAppWS.Models;
using CartAppWS.Services;
using CartAppWS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;






namespace CartAppWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _user = new UserService();
            _configuration = configuration;
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getuser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUser()
        {
            return Ok(_user.Get());
        }
        [HttpGet("getuserbyid")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUserByID(int id)
        {
            var record = _user.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.USER_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }
        #endregion

        #region Post
        [HttpPost("saveuser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SaveUser(User user)
        {
            try
            {
                ModelState.Clear();
                if (user.UserName.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.USERNAME_EMPTY.GetDescription());
                if (user.Password.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PASSWORD_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                
                if (user.IDUser == 0)
                {
                    user.CreatedDate = DateTime.Now;
                }
                user.ModifiedDate = DateTime.Now;
                int i = _user.Save(user);
                if (i > 0)
                    return Ok();
                else
                    return Constants.InternalServerError();
            }
            catch (Exception)
            {
                return Constants.InternalServerError();
            }

        }

        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            try
            {
                var record = _user.Login(login.User, login.Password);
                if (record != null && record.Status)
                    return BuildToken(record);
                else
                    return Unauthorized();
            }
            catch (Exception )
            {
                return Constants.InternalServerError();
            }
        }

        [HttpDelete("deleteuser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                User user = _user.GetById(id);
                if (user == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.USER_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    user.Status = true; //(int) Constants.Status.ELIMINADO ;
                    user.ModifiedDate = DateTime.Now;
                    int i = _user.Save(user);
                    if (i > 0)
                        return Ok();
                    else
                        return Constants.InternalServerError();
                }
            }
            catch (Exception)
            {
                return Constants.InternalServerError();
            }
        }


        private IActionResult BuildToken(User user)
        {
            string uuid = Guid.NewGuid().ToString();
            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, uuid),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["CLAVE_TOKENS"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://tierracolombianarestaurant-cartapp.com/",
                audience: "http://tierracolombianarestaurant-cartapp.com/",
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );
            //Session.Add(user.UserName, uuid);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration,
                user,
            });
        }

        #endregion
    }
}
