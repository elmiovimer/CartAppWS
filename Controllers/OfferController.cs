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

    public class OfferController : ControllerBase
    {
        private readonly IOffer _offer;

        public OfferController()
        {
            _offer = new OfferService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getoffer")]
        public IActionResult GetOffer()
        {
            return Ok(_offer.Get());
        }

        [HttpGet("getavailable")]
        public IActionResult GetAvailable()
        {
            return Ok(_offer.GetAvailable());
        }

        [HttpGet("getofferbyid")]
        public IActionResult GetOfferByID(int id)
        {
            var record = _offer.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.OFFER_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }      
        }


        #endregion



        #region Post

        [HttpPost("saveoffer")]
        public IActionResult SaveOffer(Offer offer)
        {
            try
            {
                ModelState.Clear();
                if (offer.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR,  Constants.Errors.NAME_EMPTY.GetDescription());
                if (offer.EndDate < offer.StartDate)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PERIOD_DATE.GetDescription());
                if (offer.EndTime < offer.StartTime)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PERIOD_TIME.GetDescription());
                if (offer.Price < 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRICE_NEGATIVE.GetDescription());
                if (offer.Products.Count == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NO_PRODUCTS.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (offer.IDOffer == 0)
                {
                    offer.CreatedDate = System.DateTime.Now;
                    offer.Status = (int)Constants.Status.ACTIVO;
                }
                offer.ModifiedDate = System.DateTime.Now;
                int i = _offer.Save(offer);
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

        [HttpDelete("deleteoffer")]
        public IActionResult DeleteOffer(int id)
        {
            try
            {
                Offer offer = _offer.GetById(id);
                if (offer == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.OFFER_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    offer.Status = (int)Constants.Status.ELIMINADO;
                    offer.ModifiedDate = DateTime.Now;
                    int i = _offer.Save(offer);
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
