using CyberPulse.Backend.Data;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StatusController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Status.ToListAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _context.Status.FindAsync(id);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Statu entity)
    {
        _context.Add(entity);

        await _context.SaveChangesAsync();

        return Ok(entity);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var entity = await _context.Status.FindAsync(id);

        if(entity == null)
        {
            return NotFound();
        }

        _context.Remove(entity);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut]

    private async Task<IActionResult> PutAsync(Statu entity)
    {
        _context.Update(entity);

        await _context.SaveChangesAsync();


        return Ok(entity);
    }

}
