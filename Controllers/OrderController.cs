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
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;

        public OrderController()
        {
            _order = new OrderService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getorder")]
        public IActionResult GetOrder()
        {
            return Ok(_order.Get());
        }

        [HttpGet("getorderbyid")]
        public IActionResult GetOrderByID(int id)
        {
            var record = _order.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.ORDER_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        [HttpGet("getavailableorders")]
        public IActionResult GetAvailablesOrder(int id)
        {
            return Ok(_order.GetAvailableOrders(id));
        }

        [HttpGet("getorderbyclient")]
        public IActionResult GetOrderByClient(int id)
        {
            return Ok(_order.GetByClient(id));
        }

        [HttpGet("getordersaccepted")]
        public IActionResult GetAccepted(int tracking)
        {
            return Ok(_order.TrackOrder(tracking));
        }

        [HttpPost("trackorders")]
        public IActionResult TrackOrdes(int[] ids)
        {
            return Ok(_order.TrackOrders(ids));
        }


        #endregion

        #region Post

        [HttpPost("saveorder")]
        public IActionResult SaveOrder(Order order)
        {
            try
            {
                ModelState.Clear();
                if (order.Total < 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRICE_NEGATIVE.GetDescription());
                if (order.Items.Count == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NO_PRODUCTS.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (order.IDOrder == 0)
                {
                    order.Date = System.DateTime.Now;
                    order.Status = (int)Constants.Status.ACTIVO;
                    order.Tracking = (int)Constants.Tracking.PENDIENTE;
                }
                
                int i = _order.Save(order);
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

        [HttpPost("changetrack")]
        public IActionResult ChangeTrack(OrderTracker tracker)
        {
            try
            {
                Order order = _order.GetById(tracker.IDOrder);
                if (order == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.ORDER_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    order.Tracking = tracker.Tracking;
                    //order.ModifiedDate = DateTime.Now;
                    int i = _order.Save(order);
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

        [HttpDelete("deleteorder")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                Order order = _order.GetById(id);
                if (order == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.ORDER_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    order.Status = (int)Constants.Status.ELIMINADO;
                    //order.ModifiedDate = DateTime.Now;
                    int i = _order.Save(order);
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

        private IActionResult ChargePayment(Order order)
        {
            IPaymentMethodTypes _types = new PaymentPethodTypeService();
            ICompany _company = new CompanyService();
            Company company = _company.Get();
            IPaymentMethod _method = new PaymentMethodService();
            PaymentMethod paymentMethod = _method.GetById(order.IDPaymentMethod);
            if (paymentMethod.IDClient != order.IDClient)
                return Unauthorized();
            PaymentMethodType type = _types.GetById(paymentMethod.IDPaymentMethodType);
            if (type.Card)
            {
                String r = CreditCard.Charge(company.AuthorizeNETLoginID, company.AuthorizeNETTransKey, paymentMethod.CardNumber, paymentMethod.ExpDate, order.Total);
                return Ok(r);
            }
            return Ok();
        }

        #endregion

    }
}
