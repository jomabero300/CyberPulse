using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Inve;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class LotsController : GenericController<Lot>
{
    private readonly ILotUnitOfWork _lotunitOfWork;
    public LotsController(IGenericUnitOfWork<Lot> unitOfWork, ILotUnitOfWork lotunitOfWork) : base(unitOfWork)
    {
        _lotunitOfWork = lotunitOfWork;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _lotunitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _lotunitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _lotunitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }



    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] LotDTO entity)
    {
        var response = await _lotunitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] LotDTO model)
    {
        var action = await _lotunitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _lotunitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    //[HttpGet("Combo")]
    //public async Task<IActionResult> GetComboAsync()
    //{
    //    return Ok(await _lotunitOfWork.GetComboAsync());
    //}

    //[HttpGet("Combo/{id}")]
    //public async Task<IActionResult> GetComboAsync(int id)
    //{
    //    return Ok(await _lotunitOfWork.GetComboCourseAsync(id));
    //}

    [HttpGet("Combo/{id}/{indEsta}")]
    public async Task<IActionResult> GetComboAsync(int id, bool indEsta)
    {
        return Ok(await _lotunitOfWork.GetComboAsync(id, indEsta));

    }
}
