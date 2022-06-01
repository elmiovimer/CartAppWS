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
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;

        public ProductController()
        {
            _product = new ProductService();
        }

        #region Get
        //GET : ToppingType/GetToppingType
        [HttpGet("getproduct")]
        public IActionResult GetProduct()
        {
            return Ok(_product.Get());
        }

        [HttpGet("getproductbycategory")]
        public IActionResult GetProductByCategory(int id)
        {
            return Ok(_product.GetByCategory(id));
        }

        [HttpGet("getproductbysubcategory")]
        public IActionResult GetProductBySubCategory(int id)
        {
            return Ok(_product.GetBySubCategory(id));
        }

        [HttpGet("getproductbyid")]
        public IActionResult GetProductByID(int id)
        {
            var record = _product.GetById(id);
            if (record != null)
                return Ok(record);
            else
            {
                ModelState.Clear();
                ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRODUCT_NOT_FOUND.GetDescription());
                return NotFound(ModelState);
            }
        }
        #endregion
        [HttpPost("getimages")]
        public IActionResult GetImages(int[] IDs)
        {
            return Ok(_product.GetImages(IDs));
        }

        [HttpPost("getimagesbysubcategory")]
        public IActionResult GetImagesBySubCategory(int id, DateTime date)
        {
            return Ok(_product.GetImagesBySubCategory(id, date));

        }
        #region Post
        [HttpPost("saveproduct")]
        public IActionResult SaveProduct(Product product)
        {
            try
            {
                if (product.IDProduct == 0)
                {
                    product.CreateDate = System.DateTime.Now;
                    product.ImageModifiedDate = System.DateTime.Now;
                }
                else
                {
                    Product p = _product.GetById(product.IDProduct);
                    if (product.Image != p.Image)
                        product.ImageModifiedDate = System.DateTime.Now;
                }

                product.ModifiedDate = System.DateTime.Now;
                ModelState.Clear();
                if (product.Name.Trim() == "")
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.NAME_EMPTY.GetDescription());
                if (product.IDCategory == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.CATEGORY_EMPTY.GetDescription());
                if (product.IDSubCategory == 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.SUBCATEGORY_EMPTY.GetDescription());
                if (product.Price < 0)
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRICE_NEGATIVE.GetDescription());
                if (ModelState.ErrorCount > 0)
                    return BadRequest(ModelState);

                int i = _product.Save(product);
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

        [HttpDelete("deleteproduct")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                Product product = _product.GetById(id);
                if (product == null)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(Constants.ERROR, Constants.Errors.PRODUCT_NOT_FOUND.GetDescription());
                    return NotFound(ModelState);
                }
                else
                {
                    product.Status = (int)Constants.Status.ELIMINADO;
                    product.ModifiedDate = DateTime.Now;
                    int i = _product.Save(product);
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
