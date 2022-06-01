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
    public class OrderTypeController : ControllerBase
    {
        private readonly IOrderType _type;

        public OrderTypeController()
        {
            _type = new OrderTypeService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getordertype")]
        public IActionResult GetOrderType()
        {
            return Ok(_type.Get());
        }

        [HttpGet("getavailableordertype")]
        public IActionResult GetAvailableOrderType()
        {
            return Ok(_type.GetAvailable());
        }

        [HttpGet("getordertypebyid")]
        public IActionResult GetOrderTypeByID(int id)
        {
            var record = _type.GetByID(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.ORDERTYPE_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }
        #endregion

        #region Post

        [HttpPost("saveordertype")]
        public IActionResult SaveOrderType(OrderType type)
        {
            try
            {
                ModelState.Clear();
                if (type.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                return BadRequest(ModelState);
                if (type.IDOrderType == 0)
                    type.CreatedDate = DateTime.Now;
                type.ModifiedDate = DateTime.Now;
                int i = _type.Save(type);
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

        [HttpDelete("deleteordertype")]
        public IActionResult DeleteOrderType(int id)
        {
            try
            {
                OrderType type = _type.GetByID(id);
                if (type == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.ORDERTYPE_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    type.Status = (int)Constants.Status.ELIMINADO;
                    type.ModifiedDate = DateTime.Now;
                    int i = _type.Save(type);
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
