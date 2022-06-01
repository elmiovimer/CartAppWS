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
    public class CityController : ControllerBase
    {

        private readonly ICity _city;

        public CityController()
        {
            _city = new CityService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getcity")]
        public IActionResult GetCity()
        {
            return Ok(_city.Get());
        }

        [HttpGet("getcitybystate")]
        public IActionResult GetCityByState(int id)
        {
            return Ok(_city.GetByState(id));
        }

        [HttpGet("getcitybyid")]
        public IActionResult GetCityByID(int id)
        {
            var record = _city.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.CITY_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }
        #endregion

        #region Post

        [HttpPost("savecity")]
        public IActionResult SaveCity(City city)
        {
            try
            {
                ModelState.Clear();
                if (city.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (city.IDState <= 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.STATENAME_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (city.IDCity == 0)
                    city.CreatedDate = DateTime.Now;
                city.ModifiedDate = DateTime.Now;
                int i = _city.Save(city);
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

        [HttpDelete("deletecity")]
        public IActionResult DeleteCity(int id)
        {
            try
            {
                City city = _city.GetById(id);
                if (city == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CITY_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    city.Status = (int)Constants.Status.ELIMINADO;
                    city.ModifiedDate = DateTime.Now;
                    int i = _city.Save(city);
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
