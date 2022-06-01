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
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategory _subCategory;

        public SubCategoryController()
        {
            _subCategory = new SubCategoryService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getsubcategory")]
        public IActionResult GetSubCategory()
        {
            return Ok(_subCategory.Get());
        }

        [HttpGet("getsubcategorybyid")]
        public IActionResult GetSubCategoryByID(int id)
        {
            var record = _subCategory.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.SUBCATEGORY_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }

        [HttpGet("getsubcategorybycategory")]
        public IActionResult GetSubCategoryByCategory(int id)
        {
            return Ok(_subCategory.GetByCategory(id));
        }
        #endregion

        #region Post

        [HttpPost("savesubcategory")]
        public IActionResult SaveSubCategory(SubCategory subCategory)
        {
            try
            {
                ModelState.Clear();
                if (subCategory.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (subCategory.IDCategory == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                int i = _subCategory.Save(subCategory);
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

        [HttpDelete("deletesubcategory")]
        public IActionResult DeleteSubCategory(int id)
        {
            try
            {
                SubCategory subCategory = _subCategory.GetById(id);
                if (subCategory == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.SUBCATEGORY_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    subCategory.Status = (int)Constants.Status.ELIMINADO;
                    subCategory.ModifiedDate = DateTime.Now;
                    int i = _subCategory.Save(subCategory);
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
