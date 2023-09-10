using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.EF;
using Task.EF.Models;

namespace Task.Controllers
{
    [EnableCors("cors")]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly CoreDbContext db;
        public ProductsController(CoreDbContext context)
        {
            db = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await db.Products.ToListAsync();
            return data != null ? Ok(data) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await db.Products.FindAsync(id);
            return data != null ? Ok(data) : NotFound();
        }
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Create(Product obj)
        {
            try
            {
                await db.Products.AddAsync(obj);
                return await db.SaveChangesAsync() > 0 ? Ok("Product Added") : BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Product obj)
        {
            var data = await db.Products.FindAsync(obj.Id);
            if (data == null) { return NotFound(); }
            try
            {
                data.Name = obj.Name;
                data.Qty = obj.Qty;

                return await db.SaveChangesAsync() > 0 ? Ok("Updated") : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await db.Products.FindAsync(id);
            if (data == null) { return NotFound(); }

            try
            {
                db.Products.Remove(data);
                return await db.SaveChangesAsync() > 0 ? Ok("Product Deleted") : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
