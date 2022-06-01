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
    public class StateController : ControllerBase
    {
        private readonly IState _state;

        public StateController()
        {
            _state = new StateService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getstate")]
        public IActionResult GetState()
        {
            return Ok(_state.Get());
        }

        [HttpGet("getstatebyid")]
        public IActionResult GetStateByID(int id)
        {
            var record = _state.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.STATE_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }
        #endregion

        #region Post

        [HttpPost("savestate")]
        public IActionResult SaveState(State state)
        {
            try
            {
                ModelState.Clear();
                if (state.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (state.IDState == 0)
                    state.CreatedDate = DateTime.Now;
                state.ModifiedDate = DateTime.Now;
                int i = _state.Save(state);
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

        [HttpDelete("deletestate")]
        public IActionResult DeleteState(int id)
        {
            try
            {
                State state = _state.GetById(id);
                if (state == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.STATE_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    state.Status = (int)Constants.Status.ELIMINADO;
                    state.ModifiedDate = DateTime.Now;
                    int i = _state.Save(state);
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
