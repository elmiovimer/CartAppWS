using CartAppWS.Models;
using CartAppWS.Services;
using CartAppWS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;

namespace CartAppWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethod _method;
        private readonly IPaymentMethodTypes _types;

        public PaymentMethodController()
        {
            _method = new PaymentMethodService();
            _types = new PaymentPethodTypeService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getpaymentmethod")]
        public IActionResult GetPaymentMethod()
        {
            return Ok(_method.Get());
        }

        [HttpGet("getpaymentmethodtype")]
        public IActionResult GetPaymentMethodType()
        {
            return Ok(_types.Get());
        }

        [HttpGet("getcardtype")]
        public IActionResult GetCardType(string cardnumber)
        {
            return Ok(CardTool.CardType(cardnumber));
        }

        [HttpGet("isvalid")]
        public IActionResult IsValid(string cardnumber)
        {
            return Ok(CardTool.IsValid(cardnumber));
        }

        [HttpGet("getpaymentmethodbyid")]
        public IActionResult GetPaymentMethodByID(int id)
        {
            var record = _method.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        [HttpGet("getbyclient")]
        public IActionResult GetByClient(int id)
        {
            return Ok(_method.GetByClient(id));

        }
        #endregion

        #region Post

        [HttpPost("savepaymentmethod")]
        public IActionResult SavePaymentMethod(PaymentMethod method)
        {
            try
            {
                ModelState.Clear();
                int i;
                PaymentMethodType type = _types.GetById(method.IDPaymentMethodType);
                if (type == null) 
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CREDITCARD_EMPTY.GetDescription());
                if (type.Card && method.CardNumber.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CREDITCARD_EMPTY.GetDescription());
                if (type.Card && !CardTool.IsValid(method.CardNumber))
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CREDITCARD_INCORRECT.GetDescription());
                if (!type.Card && (method.UserName.Trim() == "" || method.Password.Trim() == ""))
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PASSWORD_EMPTY.GetDescription());
                if (method.IDClient == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLIENT_NOT_FOUND.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);

                i = _method.Save(method);
                if (i > 0)
                    return Ok();
                else
                    return Constants.InternalServerError();
            }
            catch (Exception e)
            {
                return Constants.InternalServerError();
            }

        }

         [HttpPost("chargepayment")]
         /*public IActionResult ChargePayment(Payment payment)
         {
             ICompany _company = new CompanyService();
             Company company = _company.Get();
             PaymentMethod paymentMethod = _method.GetById(payment.IDPaymentMethod);
            if (paymentMethod.IDClient != payment.order.IDClient)
            {
                return Unauthorized();
            }
             PaymentMethodType type = _types.GetById(paymentMethod.IDPaymentMethodType);
             if (type.Card)
             {
                 String r = CreditCard.Charge(company.AuthorizeNETLoginID, company.AuthorizeNETTransKey, paymentMethod.CardNumber, paymentMethod.ExpDate, payment.order.Total);
                 return Ok(r);
             }
             return Ok();
         }*/

        [HttpDelete("deletepaymentmethod")]
        public IActionResult DeletePaymentMethod(int id)
        {
            try
            {
                PaymentMethod method = _method.GetById(id);
                if (method == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    method.Status = (int)Constants.Status.ELIMINADO;
                    method.ModifiedDate = DateTime.Now;
                    int i = _method.Save(method);
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
    }
}
