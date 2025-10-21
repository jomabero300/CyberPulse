using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Gene;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class StatusController : GenericController<Statu>
{
    private readonly IStatuUnitOfWork _statuUnitOfWork;
    public StatusController(IGenericUnitOfWork<Statu> unitOfWork, IStatuUnitOfWork statuUnitOfWork) : base(unitOfWork)
    {
        _statuUnitOfWork = statuUnitOfWork;
    }


    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _statuUnitOfWork.GetAsync();

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("full")]
    public async Task<IActionResult> GetAsync(string name,int nivel)
    {
        var response = await _statuUnitOfWork.GetAsync(name,nivel);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _statuUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _statuUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [AllowAnonymous]
    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _statuUnitOfWork.GetComboAsync(id));
    }
}
