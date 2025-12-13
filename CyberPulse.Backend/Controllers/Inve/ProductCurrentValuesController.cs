using CyberPulse.Backend.Helpers;
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
public class ProductCurrentValuesController : GenericController<ProductCurrentValue>
{
    private readonly IProductCurrentValueUnitOfWork _ProductCurrentValueUnitOfWork;
    private readonly IWebHostEnvironment _env;
    public ProductCurrentValuesController(IGenericUnitOfWork<ProductCurrentValue> unitOfWork, IProductCurrentValueUnitOfWork productCurrentValueUnitOfWork, IWebHostEnvironment env) : base(unitOfWork)
    {
        _ProductCurrentValueUnitOfWork = productCurrentValueUnitOfWork;
        _env = env;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _ProductCurrentValueUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _ProductCurrentValueUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _ProductCurrentValueUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }



    [HttpGet("All/{id}")]
    public async Task<IActionResult> GetAsync(double id)
    {
        var response = await _ProductCurrentValueUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }




    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] ProductCurrentValueDTO entity)
    {
        var response = await _ProductCurrentValueUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] ProductCurrentValueDTO model)
    {
        var action = await _ProductCurrentValueUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("report/{Filter}")]
    public async Task<IActionResult> GetAsync(string Filter = "")
    {
        var entity = await _ProductCurrentValueUnitOfWork.GetAsync(Filter);

        string rutaPath = _env.WebRootPath;

        var pdf = InveReportService.GenerarPdf([.. entity.Result!], rutaPath);

        return File(pdf, "application/pdf", "ProductCurrentValue.pdf");
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _ProductCurrentValueUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }
}