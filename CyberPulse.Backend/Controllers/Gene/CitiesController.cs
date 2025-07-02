using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Gene;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class CitiesController : GenericController<City>
{
    private readonly ICityUnitOfWork _cityUnitOfWork;
    private readonly ICityRepository _cityRepository;
    public CitiesController(IGenericUnitOfWork<City> unitOfWork, ICityUnitOfWork cityUnitOfWork, ICityRepository cityRepository) : base(unitOfWork)
    {
        _cityUnitOfWork = cityUnitOfWork;
        _cityRepository = cityRepository;
    }

    [AllowAnonymous]
    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _cityRepository.GetComboAsync(id));
    }
}