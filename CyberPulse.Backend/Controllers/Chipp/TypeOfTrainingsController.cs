using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Chipp;

[ApiController]
[Route("api/[controller]")]
public class TypeOfTrainingsController : GenericController<TypeOfTraining>
{
    private readonly ITypeOfTrainingUnitOfWork _typeOfTraining;

    public TypeOfTrainingsController(IGenericUnitOfWork<TypeOfTraining> unitOfWork, ITypeOfTrainingUnitOfWork typeOfTraining) : base(unitOfWork)
    {
        _typeOfTraining = typeOfTraining;
    }

    [AllowAnonymous]
    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _typeOfTraining.GetComboAsync());
    }
}
