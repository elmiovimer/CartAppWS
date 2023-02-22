using CartAppWS.Models;
using CartAppWS.Services;
using CartAppWS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cors;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace CartAppWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ClientController : ControllerBase
    {

        private readonly IClient _client;
        private readonly IConfiguration _configuration;
        public ClientController(IConfiguration configuration)
        {
            _client = new ClientService();
            _configuration = configuration;
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getclient")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetClient()
        {
            return Ok(_client.Get());
        }

        [HttpGet("getclientbyid")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetClientByID(int id)
        {
            var record = _client.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLIENT_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }


        [HttpGet("getvalidationtypes")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetValidationTypes()
        {
            return Ok(_client.GetValidationTypes());
        }
        [HttpGet("validate")]

        public ContentResult Validate(string validation)
        {
            Client c = _client.Validate(validation);
            if (c != null)
                return Welcome(c.FirstName);
            else
                return WrongAnswer();
        }

        [HttpPost("subirfoto")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SubirFoto(Company foto)
        {
            ICompany _company = new CompanyService();
            _company.Update(foto.Logo);
            return Ok();
        }


        private ContentResult Welcome(string nombre)
        {
            ICompany _company = new CompanyService();
            string img = _company.Get().Logo;
            string page = $"<img style='margin: auto; width: 70%; height: auto; display: block; max-width: 400px;' src='{img}'>" +
                $"<h2 style = 'color: #BD9B60; text-align:center;'> Welcome, {nombre}! </h2>" +
                "<p style = 'color:#3F2021; text-align: center;font-size: x-large;'> Your account is activated now.<br/> Thank you for join us.<br/> Now you can access to our applicacion </p>";
            return base.Content(page, "text/html");
        }

        private ContentResult WrongAnswer()
        {

            ICompany _company = new CompanyService();
            Company c = _company.Get();
            string img = c.Logo;
            string tel = c.Phone;
            string mail = c.Email;
            string page = $"<img style='margin: auto; width: 70%; height: auto; display: block; max-width: 400px;' src='{img}'>" +
                $"<h2 style='color: #FF4500; text-align:center;'>Something went wrong!</h2>" +
                $"<div style = 'margin: auto; max-width: 400px;'> <p style = 'color:#3F2021; text-align: center;font-size: x-large;'>" +
                $"Link should be expired. Please try login again and a new email will be sent to you with a new validation link. " +
                $"<br/>If it does not work please contact us at <a href= 'tel:{tel}' >{tel}</a> or by email at <a href= 'mailto:{mail}'>{mail}</a></p></div>";
            return base.Content(page, "text/html");
        }
        #endregion

        #region Post
        [HttpPost("saveclient")]
        public IActionResult SaveClient(Client client, int validationType)
        {
            try
            {
                ModelState.Clear();
                client.Email = client.Email.Trim();
                if (client.FirstName.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.FIRSTNAME_EMPTY.GetDescription());
                if (client.LastName.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.LASTNAME_EMPTY.GetDescription());
                if (client.Phone.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PHONE_EMPTY.GetDescription());
                if (client.Email.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.EMAIL_EMPTY.GetDescription());
                if (client.IDClient == 0 && client.Password.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PASSWORD_EMPTY.GetDescription());
                if (client.IDClient == 0 && _client.GetByEmail(client.Email) != null)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.DUPLICATE_EMAIL.GetDescription());
                if (!Mail.IsValid(client.Email))
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.BAD_EMAIL_FORMAT.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (client.IDClient == 0)
                {
                    client.CreatedDate = DateTime.Now;
                    client.Status = (int)Constants.Status.NO_VALIDADO;
                }
                client.ModifiedDate = DateTime.Now;
                int i = _client.Save(client);
                if (i > 0)
                {
                    Validar(client.Email, validationType);
                    client = _client.GetByEmail(client.Email);
                    return Ok(client.IDClient.ToString());
                }
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
            var record = _client.Login(login.User, login.Password);
            if (record != null)
                return BuildToken(record);
            else
            {
                return NotFound();
            }
        }


        [HttpPost("sendvalidation")]
        public IActionResult SendValidation(string email, int Type)
        {
            return Validar(email, Type);
        }

        private IActionResult Validar(string email, int Type)
        {
            try
            {
                Client client = _client.GetByEmail(email);
                ICompany _company = new CompanyService();
                Company company = _company.Get();
                if (client == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLIENT_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);

                }
                ClientValidation validation = _client.GenerateValidation(client.IDClient);
                if (validation != null)
                {
                    switch (Type)
                    {
                        case (int)Constants.ValidationType.SMS:
                            SMS.SendSMS(company.SMSUser, company.SMSPassword, company.SMSPhone, client.Phone, company.WSLink + validation.GetLink());
                            break;
                        case (int)Constants.ValidationType.EMAIL:
                            //Mail.SendMail(company.Email, client.Email, "Validation - Tierra Colombiana", "<html><body><" company.WSLink + validation.GetLink(), company.SMTPServer, company.EmailPassword, company.SMTPPort, company.StartTLS.ToUpper().Equals("TRUE"));
                            Mail.SendMail(company.Email, client.Email, "<html><body>Validation - Tierra Colombiana", "<a href='" + company.WSLink + validation.GetLink() + "'>Clic aquí para activar</a></body></html>", company.SMTPServer, company.EmailPassword, company.SMTPPort, company.StartTLS.ToUpper().Equals("TRUE"));
                            break;
                    }
                }

                return Ok();

            }
            catch (Exception)
            {
                return Constants.InternalServerError() ;
            }
        }


        [HttpPost("resetpassword")]
        public IActionResult ResetPassword(string email, int Type)
        {
            try
            {
                Client client = _client.GetByEmail(email);
                ICompany _company = new CompanyService();
                Company company = _company.Get();
                if (client == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLIENT_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                String password = _client.GeneratePassword();
                int i = _client.ChangePassword(client.IDClient, password);
                if (password != null)
                {
                    switch (Type)
                    {
                        case (int)Constants.ValidationType.SMS:
                            SMS.SendSMS(company.SMSUser, company.SMSPassword, company.SMSPhone, client.Phone, ResetPasswordSMS(password, company.Name));
                            break;
                        case (int)Constants.ValidationType.EMAIL:
                            Mail.SendMail(company.Email, client.Email, "Reset Password - Tierra Colombiana", ResetPasswordMessage(client.FirstName, password, company.Logo), company.SMTPServer, company.EmailPassword, company.SMTPPort, company.StartTLS.ToUpper().Equals("TRUE"));
                            break;
                    }
                }

                return Ok();

            }
            catch (Exception )
            {
                return Constants.InternalServerError();
            }
        }

        private string ResetPasswordMessage(string name, string password, string img)
        {
            String var = $"<html><head></head><body><img style='margin: auto; width: 100%; height: auto; display: block; max-width: 400px;' src='{img.Trim()}'>" +
                $"<h2 style = 'color: #BD9B60; text-align:center;'> Hi {name},</h2>" +
                "<div style = 'margin: auto; max-width: 400px;'> <p style = 'color:#3F2021; text-align: center;font-size: x-large;'>" +
                "We're sending you this email because you requested a password reset.</p>" +
                $"<p style = 'color:#3F2021; text-align: center;font-size: x-large;'> We have asigned to you a temporary password: <strong>{password}</strong>.</p>" +
                $"<p style='color:#3F2021; text-align: center;font-size: x-large;'> Please login to your account and set up a new password.</p></div></body></html>";
            Console.WriteLine(var);
            return $"<html><head></head><body> \n<img style='margin: auto; width: 100%; height: auto; display: block; max-width: 400px;' src='http://tierracolombiana-001-site2.itempurl.com/splash.png'>" +
                $"\n <h2 style = 'color: #BD9B60; text-align:center;'> Hi {name},</h2>" +
                "\n <div style = 'margin: auto; max-width: 400px;'> <p style = 'color:#3F2021; text-align: center;font-size: x-large;'>" +
                "\n We're sending you this email because you requested a password reset.</p>" +
                $"\n <p style = 'color:#3F2021; text-align: center;font-size: x-large;'> We have asigned to you a temporary password: <strong>{password}</strong>.</p>" +
                $"\n <p style='color:#3F2021; text-align: center;font-size: x-large;'> Please login to your account and set up a new password.</p></div></body></html>";
        }

        private string ResetPasswordSMS(string password, string companyname)
        {
            return $"{password} is your temporary password to reset your {companyname}'s account";
        }

        [HttpPost("changepassword")]
        public IActionResult ChangePassword(PasswordChange password)
        {
            try
            {
                int i = _client.ChangePassword(password.IDClient, password.OldPassword, password.NewPassword);
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

        [HttpDelete("deleteclient")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                Client client = _client.GetById(id);
                if (client == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLIENT_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);

                }
                else
                {
                    client.Status = (int)Constants.Status.ELIMINADO;
                    client.ModifiedDate = DateTime.Now;
                    int i = _client.Save(client);
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

        #endregion

        private IActionResult BuildToken(Client client)
        {
            string uuid = Guid.NewGuid().ToString();
            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.UniqueName, client.IDClient.ToString()),
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
                client = client
            });
        }

    }
}
