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
    public class ToppingTypeController : ControllerBase
    {
        private readonly IToppingType _toppingType;

        public ToppingTypeController()
        {
            _toppingType = new ToppingTypeService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("gettoppingtype")]
        public IActionResult GetToppingType()
        {
            return Ok(_toppingType.Get());
        }

        [HttpGet("gettoppingtypebyid")]
        public IActionResult GetToppingTypeByID(int id)
        {
            var record = _toppingType.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.TOPPINGTYPE_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        [HttpGet("gettoppinggroup")]
        public IActionResult GetToppingGroup()
        {
            return Ok(_toppingType.GetGroups());
        }

        [HttpGet("gettoppinggroupbyid")]
        public ToppingGroup GetToppingGroupByID(int id)
        {
            return _toppingType.GetGroup(id);

        }
        #endregion

        #region Post
        [HttpPost("savetoppingtype")]
        public IActionResult SaveToppingType(ToppingType toppingType)
        {
            try
            {
                ModelState.Clear();
                if (toppingType.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (toppingType.IDToppingType == 0)
                {
                    toppingType.Status = (int)Constants.Status.ACTIVO;
                    toppingType.CreatedDate = DateTime.Now;
                }
                toppingType.ModifiedDate = DateTime.Now;
                int i = _toppingType.Save(toppingType);
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
        [HttpDelete("deletetoppingtype")]
        public IActionResult DeleteToppingType(int id)
        {
            try
            {
                ToppingType toppingType = _toppingType.GetById(id);
                if (toppingType == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.TOPPINGTYPE_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    toppingType.Status = (int)Constants.Status.ELIMINADO;
                    toppingType.ModifiedDate = DateTime.Now;
                    int i = _toppingType.Save(toppingType);
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
