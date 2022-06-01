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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CartAppWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClaimController : ControllerBase
    {
        private readonly IClaim _claim;

        public ClaimController()
        {
            _claim = new ClaimService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getclaim")]
        public IActionResult GetClaim()
        {
            return Ok(_claim.Get());
        }

        [HttpGet("getclaimbyid")]
        public IActionResult GetClaimByID(int id)
        {
            var record = _claim.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLAIM_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        [HttpGet("getbyorder")]
        public IActionResult GetByOrder(int id)
        {
            return Ok(_claim.GetByOrder(id));
        }
        #endregion

        #region Post

        [HttpPost("saveclaim")]
        public IActionResult SaveClaim(Claim claim)
        {
            try
            {
                ModelState.Clear();
                if (claim.Comment.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                int i = _claim.Save(claim);
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

        [HttpDelete("deleteclaim")]
        public IActionResult DeleteClaim(int id)
        {
            try
            {
                Claim claim = _claim.GetById(id);
                if (claim == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CLAIM_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    claim.Status = (int)Constants.Status.ELIMINADO;
                    //claim.ModifiedDate = DateTime.Now;
                    int i = _claim.Save(claim);
                    if (i > 0)
                        return Ok();
                    else
                        return Constants.InternalServerError();
                }
            }
            catch (Exception e)
            {
                return Constants.InternalServerError();
            }
        }

        #endregion

    }
}
