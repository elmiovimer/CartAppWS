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
    [EnableCors]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoryController()
        {
            _category = new CategoryService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getcategory")]
        public IActionResult GetCategory()
        {
            return Ok(_category.Get());
        }

        [HttpGet("getcategorybyid")]
        public IActionResult GetCategoryByID(int id)
        {
            var record = _category.GetById(id);
            if(record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }     
        }

        [HttpGet("getcategorylist")]
        public IActionResult GetCategoryList()
        {
            return Ok(_category.GetCategoryList());
        }

        [HttpGet("getcategorylistbyid")]
        public IActionResult GetCategoryListByID(int id)
        {
            var record = _category.GetCategoryListByID(id);
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

        [HttpPost("savecategory")]
        public IActionResult SaveCategory(Category category)
        {
            try
            {
                ModelState.Clear();
                if (category.Name.Trim() == "")
                   ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);
                
                int i = _category.Save(category);
                if (i > 0)
                    return Ok();
                else
                    return Constants.InternalServerError() ;
            }
            catch (Exception)
            {
                return Constants.InternalServerError();
            }

        }

        [HttpDelete("deletecategory")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                Category category = _category.GetById(id);
                if (category == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    category.Status = (int)Constants.Status.ELIMINADO;
                    category.ModifiedDate = DateTime.Now;
                    int i = _category.Save(category);
                    if (i > 0)
                        return Ok();
                    else
                        return Constants.InternalServerError();
                }
            }
            catch (Exception )
            {
                return Constants.InternalServerError();
            }
        }

        #endregion


    }
}
