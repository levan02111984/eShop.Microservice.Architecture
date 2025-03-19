using Ecommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products =  await productInterface.GetAllAsync();
            if (!products.Any()) { 
                return NotFound("No products detected in the database");
            }

            //Converting Entity to DTO
            var(_, list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No product found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await productInterface.FindByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product requested not found");
            }
            //Converting Entity To DTO
            var (_product, _) = ProductConversion.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound("Product not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO)
        {
            //check model state is all data annotations are passed.
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            //Converting DTO to Entity to save to database
            var currentEntity = ProductConversion.ConvertToEntity(productDTO);
            var updateResult = await productInterface.CreateAsync(currentEntity);

            return updateResult.Flag is true ? Ok(updateResult) : BadRequest(updateResult);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO productDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var currentEntity = ProductConversion.ConvertToEntity(productDTO);
            var updateResult = await productInterface.UpdateAsync(currentEntity);

            return updateResult.Flag is true ? Ok(updateResult) : BadRequest(updateResult);

        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProdcut(ProductDTO productDTO)
        {
            var currentEntity = ProductConversion.ConvertToEntity(productDTO);
            var deleteResult = await productInterface.DeleteAsync(currentEntity);

            return deleteResult.Flag is true ? Ok(deleteResult) : BadRequest(deleteResult);

        }
    }
}
