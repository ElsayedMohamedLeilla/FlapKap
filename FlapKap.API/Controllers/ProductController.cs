using FlapKap.Contract.BusinessLogic;
using FlapKap.Models.DTOs.Products;
using FlapKap.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlapKap.API.Controllers
{
    [Route("api/[controller]/[action]"), ApiController]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductBL productBL;

        public ProductController(IProductBL _productBL)
        {
            productBL = _productBL;
        }
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> Create([FromBody] CreateProductModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await productBL.Create(model);
            return Success(result, message: "Done Create Product Successfully");
        }
        [HttpPut]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> Update([FromBody] UpdateProductModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await productBL.Update(model);
            return Success(result, message: "Done Update Product Successfully");
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetProductsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var productsresponse = await productBL.Get(criteria);

            return Success(productsresponse.Products, productsresponse.TotalCount);
        }
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> GetById([FromQuery] int productId)
        {
            if (productId < 1)
            {
                return BadRequest();
            }
            return Success(await productBL.GetById(productId));
        }
        [HttpDelete]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> Delete(int productId)
        {
            if (productId < 1)
            {
                return BadRequest();
            }
            return Success(await productBL.Delete(productId));
        }
    }
}