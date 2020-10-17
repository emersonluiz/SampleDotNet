using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleEndPoint.Models;

namespace SampleEndPoint.Controllers
{
	[ApiController]
    [Route("categories")]
    public class CategoryController: ControllerBase
    {
        [HttpGet]
		[Route("")]
		public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
		{
			var categories = await context.Categories.ToListAsync();
			return categories;
		}

		[HttpGet]
		[Route("{id:long}")]
		public Task<ActionResult<Category>> GetById([FromServices] DataContext context, long id)
		{
			var category = getCategory(context, id);
			return category;
		}

		[HttpPost]
		[Route("")]
		public async Task<ActionResult<Category>> Post([FromServices] DataContext context, [FromBody] Category model)
		{
			if(ModelState.IsValid)
			{
				context.Categories.Add(model);
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
		public async Task<ActionResult<Category>> Put([FromServices] DataContext context, long id, [FromBody] Category model)
		{
			var category = getCategory(context, id);

			if(ModelState.IsValid)
			{
				model.Id = category.Result.Value.Id;
				context.Categories.Update(model);
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
			var category = getCategory(context, id);

			context.Categories.Remove(category.Result.Value);
			await context.SaveChangesAsync();

			return NoContent();
		}

		private async Task<ActionResult<Category>> getCategory(DataContext context, long id)
		{
			var category = await context.Categories
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			return category;
		}
    }
}