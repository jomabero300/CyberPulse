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
public class InvProgramsController : GenericController<InvProgram>
{

    private readonly IInvProgramUnitOfWork _invProgramUnitofWork;
    private readonly IWebHostEnvironment _env;
    public InvProgramsController(IGenericUnitOfWork<InvProgram> unitOfWork, IInvProgramUnitOfWork invProgramUnitofWork, IWebHostEnvironment env) : base(unitOfWork)
    {
        _invProgramUnitofWork = invProgramUnitofWork;
        _env = env;
    }

    [HttpGet("full/{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _invProgramUnitofWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _invProgramUnitofWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _invProgramUnitofWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }



    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] InvProgramDTO entity)
    {
        var response = await _invProgramUnitofWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] InvProgramDTO model)
    {
        var action = await _invProgramUnitofWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _invProgramUnitofWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("report/{Filter}")]
    public async Task<IActionResult> GetAsync(string Filter = "")
    {
        var entity = await _invProgramUnitofWork.GetAsync(Filter);

        string rutaPath = _env.WebRootPath;

        var pdf = InveReportService.GenerarPdf([.. entity.Result!], rutaPath);

        return File(pdf, "application/pdf", "Family.pdf");
    }

    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _invProgramUnitofWork.GetComboAsync());
    }

    //[HttpGet("ProgramLot/{id}")]
    //public async Task<ActionResult> GetAsync(int id, bool lb=false)
    //{
    //    var response = await _invProgramUnitofWork.GetAsync(id,lb);

    //    if (response.WasSuccess)
    //    {
    //        return Ok(response.Result);
    //    }

    //    return BadRequest();

    //}
}
