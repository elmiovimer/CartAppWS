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
    [EnableCors]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AddressController : ControllerBase
    {
        private readonly IAddress _address;

        public AddressController()
        {
            _address = new AddressService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getaddress")]
        public IActionResult GetAddress()
        {
            return Ok(_address.Get());
        }

        [HttpGet("getaddressbyid")]
        public IActionResult GetAddressByID(int id)
        {
            var record = _address.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.ADDRESS_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }


        //public List<Address> GetAdrressByClient(int id)
        [HttpGet("getaddressbyclient")]
        public IActionResult GetAddressByClient(int id)
        {
           return Ok(_address.GetByClient(id));
            
        }
        #endregion

        #region Post

        [HttpPost("saveaddress")]
        public IActionResult SaveAddress(Address address)
        {
            ModelState.Clear();
            try
            {
                if (address.Name.Trim() == "")
                     ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());

                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                if (address.IDAddress == 0)
                {
                    address.CreatedDate = DateTime.Now;
                }
                address.ModifiedDate = DateTime.Now;
                int i = _address.Save(address);
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

        [HttpPost("setbydefault")]
        public IActionResult SetByDefault(int id)
        {
            try
            {
                Address address = _address.GetById(id);
                if (address == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.ADDRESS_NOT_FOUND.GetDescription());
                }
                if(ModelState.ErrorCount > 0)
                    return NotFound(ModelState);
                else
                {
                    address.ByDefault = true;
                    int i = _address.Save(address);
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


        [HttpDelete("deleteaddress")]
        public IActionResult DeleteAddress(int id)
        {
            try
            {
                Address address = _address.GetById(id);
                if (address == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.ADDRESS_NOT_FOUND.GetDescription());
                }
                if (ModelState.ErrorCount > 0)
                    return NotFound(ModelState);
                else
                {
                    address.Status = (int)Constants.Status.ELIMINADO;
                    address.ModifiedDate = DateTime.Now;
                    int i = _address.Save(address);
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
