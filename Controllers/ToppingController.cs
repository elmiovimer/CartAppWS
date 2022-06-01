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
    public class ToppingController : ControllerBase
    {
        private readonly ITopping _topping;

        public ToppingController()
        {
            _topping = new ToppingService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("gettopping")]
        public IActionResult GetTopping()
        {
            return Ok(_topping.Get());
        }

        [HttpGet("gettoppingbyid")]
        public IActionResult GetToppingByID(int id)
        {
            var record = _topping.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRODUCT_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        [HttpGet("gettoppingbytoppingtype")]
        public IActionResult GetToppingByToppingType(int id)
        {
            return Ok(_topping.GetByToppingType(id));
        }
        #endregion

        #region Post
        [HttpPost("savetopping")]
        public IActionResult SaveTopping(Topping topping)
        {
            try
            {
                ModelState.Clear();
                if (topping.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (topping.IDToppingType == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.TOPPINGTYPE_EMPTY.GetDescription());

                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (topping.IDTopping == 0)
                {
                    topping.Status = (int)Constants.Status.ACTIVO;
                    topping.CreatedDate = DateTime.Now;
                }
                topping.ModifiedDate = DateTime.Now;

                int i = _topping.Save(topping);
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

        [HttpDelete("deletetopping")]
        public IActionResult DeleteTopping(int id)
        {
            try
            {
                Topping topping = _topping.GetById(id);
                if (topping == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRODUCT_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    topping.Status = (int)Constants.Status.ELIMINADO;
                    topping.ModifiedDate = DateTime.Now;
                    int i = _topping.Save(topping);
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
