using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Gene;

[ApiController]
[Route("api/[controller]")]
public class NeighborhoodsController : GenericController<Neighborhood>
{
    private readonly INeighborhoodUnitOfWork _neighborhoodUnitOf;
    public NeighborhoodsController(IGenericUnitOfWork<Neighborhood> unitOfWork, INeighborhoodUnitOfWork neighborhoodUnitOf) : base(unitOfWork)
    {
        _neighborhoodUnitOf = neighborhoodUnitOf;
    }

    [AllowAnonymous]
    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _neighborhoodUnitOf.GetComboAsync(id));
    }

}
