
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
    public TypeOfPoblationsController(IGenericUnitOfWork<TypeOfPoblation> unitOfWork, ITypeOfPoblationUnitOfWork typeOfPoblation) : base(unitOfWork)
    {
        _typeOfPoblation = typeOfPoblation;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response=await _typeOfPoblation.GetAsync();

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("full/{filter}")]
    public async Task<IActionResult> GetAsync(string filter)
    {
        return Ok(await _typeOfPoblation.GetAsync(filter));
    }
}
