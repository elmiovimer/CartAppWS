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

    public class EventController : ControllerBase
    {
        private readonly IEvent _event;

        public EventController()
        {
            _event = new EventService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getevent")]
        public IActionResult GetEvent()
        {
            return Ok(_event.Get());
        }

        [HttpGet("getavailable")]
        public IActionResult GetAvailable()
        {
            
            return Ok(_event.GetAvailable());

        }

        [HttpGet("geteventbyid")]
        public IActionResult GetEventByID(int id)
        {
            var record = _event.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        #endregion

        #region Post

        [HttpPost("saveevent")]
        public IActionResult SaveEvent(Event e)
        {
            try
            {
                ModelState.Clear();
                if (e.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (e.IDEvent == 0)
                {
                    e.CreatedDate = DateTime.Now;
                }
                e.ModifiedDate = DateTime.Now;
                int i = _event.Save(e);
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

        [HttpDelete("deleteevent")]
        public IActionResult DeleteEvent(int id)
        {
            try
            {
                Event e = _event.GetById(id);
                if (e == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    e.Status = (int)Constants.Status.ELIMINADO;
                    e.ModifiedDate = DateTime.Now;
                    int i = _event.Save(e);
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
