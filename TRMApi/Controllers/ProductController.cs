﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRMApi.Models;
using TRMDataAccessLibrary.DataAccess;
using TRMDataAccessLibrary.Models;

namespace TRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        private readonly IProductData _productData;

        public ProductController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet]
        public List<ProductModel> GetAllProducts()
        {
            return _productData.GetAllProducts();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModel product)
        {
            if (ModelState.IsValid)
            {
                _productData.CreateProduct(product);
                return Ok();
            }

            return BadRequest();
        }
    }
}
