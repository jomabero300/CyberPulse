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
public class UnitMeasurementsController : GenericController<UnitMeasurement>
{
    private readonly IUnitMeasurementUnitOfWork _unitMeasurementUnitOfWork;
    public UnitMeasurementsController(IGenericUnitOfWork<UnitMeasurement> unitOfWork, IUnitMeasurementUnitOfWork unitMeasurementUnitOfWork) : base(unitOfWork)
    {
        _unitMeasurementUnitOfWork = unitMeasurementUnitOfWork;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _unitMeasurementUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _unitMeasurementUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _unitMeasurementUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] UnitMeasurementDTO entity)
    {
        var response = await _unitMeasurementUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }
    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] UnitMeasurementDTO model)
    {
        var action = await _unitMeasurementUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _unitMeasurementUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _unitMeasurementUnitOfWork.GetComboAsync());
    }
}
