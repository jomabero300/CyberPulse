
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Chipp;

[ApiController]
[Route("api/[controller]")]
public class TypeOfPoblationsController : GenericController<TypeOfPoblation>
{
    private readonly ITypeOfPoblationUnitOfWork _typeOfPoblation;
    private readonly ITypeOfPoblationRepository _typeOfPoblationRepository;
    public TypeOfPoblationsController(IGenericUnitOfWork<TypeOfPoblation> unitOfWork, ITypeOfPoblationUnitOfWork typeOfPoblation, ITypeOfPoblationRepository typeOfPoblationRepository) : base(unitOfWork)
    {
        _typeOfPoblation = typeOfPoblation;
        _typeOfPoblationRepository = typeOfPoblationRepository;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _typeOfPoblation.GetAsync();

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("full/{filter}")]
    public async Task<IActionResult> GetAsync(string filter)
    {
        return Ok(await _typeOfPoblationRepository.GetAsync(filter));
    }
}
