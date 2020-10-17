using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleEndPoint.Models;

namespace SampleEndPoint.Controllers
{
	[ApiController]
    [Route("products")]
    public class ProductController: ControllerBase
    {
        [HttpGet]
		[Route("")]
		public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
		{
			var products = await context.Products.Include(x => x.Category).ToListAsync();
			return products;
		}

		[HttpGet]
		[Route("{id:long}")]
		public Task<ActionResult<Product>> GetById([FromServices] DataContext context, long id)
		{
			var product = getProduct(context, id);
			return product;
		}

		[HttpGet]
		[Route("category/{id:long}")]
		public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, long id)
		{
			var products = await context.Products
			.Include(x => x.Category)
			.AsNoTracking()
			.Where(x => x.CategoryId == id)
			.ToListAsync();

			return products;
		}

		private async Task<ActionResult<Product>> getProduct(DataContext context, long id)
		{
			var product = await context.Products
			.Include(x => x.Category)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			return product;
		}

		[HttpPost]
		[Route("")]
		public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
		{
			if(ModelState.IsValid)
			{
				context.Products.Add(model);
				await context.SaveChangesAsync();
				return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
			}
			else
			{
				return BadRequest(ModelState);
			}
		}

		[HttpPut]
		[Route("{id:long}")]
		public async Task<ActionResult<Product>> Put([FromServices] DataContext context, long id, [FromBody] Product model)
		{
			var product = getProduct(context, id);

			if(ModelState.IsValid)
			{
				model.Id = product.Result.Value.Id;
				context.Products.Update(model);
				await context.SaveChangesAsync();
				return NoContent();
			}
			else
			{
				return BadRequest(ModelState);
			}
		}

		[HttpDelete("{id:long}")]
		public async Task<ActionResult<Category>> Delete([FromServices] DataContext context, long id)
		{
			var product = getProduct(context, id);

			context.Products.Remove(product.Result.Value);
			await context.SaveChangesAsync();

			return NoContent();
		}
    }
}