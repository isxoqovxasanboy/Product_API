﻿using Microsoft.AspNetCore.Mvc;
using Product_API.Models;
using Product_API.Repositories;

namespace Product_API.Controllers
{
    [Route("Products/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            return Ok(_repository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product? product)
        {
            return Ok(_repository.Add(product));
        }
    }
}
