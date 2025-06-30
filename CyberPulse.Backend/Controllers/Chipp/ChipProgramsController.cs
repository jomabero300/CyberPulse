
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Chipp;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ChipProgramsController : GenericController<ChipProgram>
{
    private readonly IChipProgramUnitOfWork _chipProgramUnitOf;

    public ChipProgramsController(IGenericUnitOfWork<ChipProgram> unitOfWork, IChipProgramUnitOfWork chipProgramUnitOf) : base(unitOfWork)
    {
        _chipProgramUnitOf = chipProgramUnitOf;
    }

    [AllowAnonymous]
    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _chipProgramUnitOf.GetComboAsync());
    }
}
