using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Inve;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ProductQuotationBodiesController : GenericController<ProductQuotationBodyDTO>
{
    private readonly IProductQuotationBodyUnitOfWork _productQuotationBodyUnitOfWork;
    public ProductQuotationBodiesController(IGenericUnitOfWork<ProductQuotationBodyDTO> unitOfWork, IProductQuotationBodyUnitOfWork productQuotationBodyUnitOfWork) : base(unitOfWork)
    {
        _productQuotationBodyUnitOfWork = productQuotationBodyUnitOfWork;
    }
    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _productQuotationBodyUnitOfWork.GetComboAsync(id));
    }
}
