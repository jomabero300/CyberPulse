using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Gene;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class IvasController : GenericController<Iva>
{
    private readonly IIvaUnitOfWork _ivaUnitOfWork;
    public IvasController(IGenericUnitOfWork<Iva> unitOfWork, IIvaUnitOfWork ivaUnitOfWork) : base(unitOfWork)
    {
        _ivaUnitOfWork = ivaUnitOfWork;
    }


    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _ivaUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _ivaUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _ivaUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] IvaDTO entity)
    {
        var response = await _ivaUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] IvaDTO model)
    {
        var action = await _ivaUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _ivaUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _ivaUnitOfWork.GetComboAsync());
    }
}