﻿using System.Collections.Generic;
using FlixOne.Common.Models;
using FlixOne.CQRS.Commands.Command;
using FlixOne.CQRS.Helper;
using FlixOne.CQRS.Queries.Query;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlixOne.API.Controllers
{
    /// <inheritdoc />
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Product> Products()
        {
            var query = new ProductQuery();
            var handler = ProductQueryHandlerFactory.Build(query);
            return handler.Get();
        }
        /// <summary>
        /// Retrieve a product by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Product Get(string id)
        {
            var query = new SingleProductQuery(id.ToValidGuid());
            var handler = ProductQueryHandlerFactory.Build(query);
            return handler.Get();
        }
        /// <summary>
        /// Saeve Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            var command = new SaveProductCommand(product);
            var handler = ProductCommandHandlerFactory.Build(command);
            var response = handler.Execute();
            if (!response.Success) return StatusCode(500, response);
            product.Id = response.Id;
            return Ok(product);

        }
        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var command = new DeleteProductCommand(id.ToValidGuid());
            var handler = ProductCommandHandlerFactory.Build(command);
            var response = handler.Execute();
            if (!response.Success) return StatusCode(500, response);
            return Ok(response);
        }
    }
}