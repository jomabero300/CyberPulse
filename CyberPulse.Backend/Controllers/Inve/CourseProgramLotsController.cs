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
public class CourseProgramLotsController : GenericController<CourseProgramLot>
{
    private readonly ICourseProgramLotUnitOfWork _courseLotUnitOfWork;
    public CourseProgramLotsController(IGenericUnitOfWork<CourseProgramLot> unitOfWork, ICourseProgramLotUnitOfWork courseLotUnitOfWork) : base(unitOfWork)
    {
        _courseLotUnitOfWork = courseLotUnitOfWork;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _courseLotUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _courseLotUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _courseLotUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }



    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] CourseProgramLotDTO entity)
    {
        var response = await _courseLotUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] CourseProgramLotDTO entity)
    {
        var action = await _courseLotUnitOfWork.UpdateAsync(entity);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _courseLotUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _courseLotUnitOfWork.GetComboAsync(id));
    }


}
