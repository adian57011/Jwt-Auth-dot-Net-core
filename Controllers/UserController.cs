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
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly CoreDbContext db;

        public UserController(CoreDbContext context) {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult>Get()
        {
            try
            {
                var data = await db.Users.ToListAsync();
                return data == null ? NotFound() : Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>Get(int id)
        {
            var data = await db.Users.FindAsync(id);
            return data != null ? Ok(data) : NotFound();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(User obj)
        {
            await db.Users.AddAsync(obj);
            return await db.SaveChangesAsync() > 0? Ok(obj):BadRequest("User addition failed");   
        }
        [HttpPatch]
        public async Task<IActionResult>Update(User obj)
        {
            var prev = await db.Users.FindAsync(obj.Id);
            if(prev == null) { return  NotFound(); }
            try
            {
                prev.Name = obj.Name;
                prev.Username = obj.Username;
                prev.Password = obj.Password;
                return await db.SaveChangesAsync() > 0 ? Ok(prev) : Problem();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Remove(int id)
        {
            var ex = await db.Users.FindAsync(id);
            if(ex == null) { return NotFound(); }
            try
            {
                 db.Users.Remove(ex);
                return await db.SaveChangesAsync()>0 ? Ok("User Deleted") : Problem();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}
