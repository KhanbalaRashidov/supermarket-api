using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermarket.API.Domain.Services;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public ProductsController(IServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all existing products according to query filters.
        /// </summary>
        /// <returns>List of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(QueryResultResource<ProductResource>), 200)]
        public async Task<QueryResultResource<ProductResource>> ListAsync([FromQuery] ProductsQueryResource query)
        {
            var productsQuery = _mapper.Map<ProductsQuery>(query);
            var queryResult = await _serviceManager.ProductService.ListAsync(productsQuery);

            return _mapper.Map<QueryResultResource<ProductResource>>(queryResult);
        }

        /// <summary>
        /// Saves a new product.
        /// </summary>
        /// <param name="resource">Product data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveProductResource resource)
        {
            var product = _mapper.Map<Product>(resource);
            var result = await _serviceManager.ProductService.SaveAsync(product);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message!));
            }

            var productResource = _mapper.Map<ProductResource>(result.Resource!);
            return Ok(productResource);
        }

        /// <summary>
        /// Updates an existing product according to an identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <param name="resource">Product data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveProductResource resource)
        {
            var product = _mapper.Map<Product>(resource);
            var result = await _serviceManager.ProductService.UpdateAsync(id, product);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message!));
            }

            var productResource = _mapper.Map<ProductResource>(result.Resource!);
            return Ok(productResource);
        }

        /// <summary>
        /// Deletes a given product according to an identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _serviceManager.ProductService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message!));
            }

            var categoryResource = _mapper.Map<ProductResource>(result.Resource!);
            return Ok(categoryResource);
        }
    }
}