using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Chipp;

[ApiController]
[Route("api/[controller]")]
public class TrainingProgramsController : GenericController<TrainingProgram>
{
    private readonly ITrainingProgramUnitOfWork _trainingProgramUnitOf;
    private readonly ITrainingProgramRepository _trainingProgramRepository;
    public TrainingProgramsController(IGenericUnitOfWork<TrainingProgram> unitOfWork, ITrainingProgramUnitOfWork trainingProgramUnitOf, ITrainingProgramRepository trainingProgramRepository) : base(unitOfWork)
    {
        _trainingProgramUnitOf = trainingProgramUnitOf;
        _trainingProgramRepository = trainingProgramRepository;
    }


    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _trainingProgramUnitOf.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _trainingProgramUnitOf.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [AllowAnonymous]
    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _trainingProgramRepository.GetComboAsync());
    }

}
