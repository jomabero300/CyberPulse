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
public class ProductQuotationsController : GenericController<ProductQuotation>
{
    private readonly IProductQuotationUnitOfWork _productQuotationUnitOf;

    public ProductQuotationsController(IGenericUnitOfWork<ProductQuotation> unitOfWork, IProductQuotationUnitOfWork productQuotationUnitOf) : base(unitOfWork)
    {
        _productQuotationUnitOf = productQuotationUnitOf;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _productQuotationUnitOf.GetAsync(id);

        if (response.WasSuccess)
        {

            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _productQuotationUnitOf.GetAsync(pagination);

        if (response.WasSuccess)
        {
            var results = response.Result!.Select(pq => new ProductQuotationPurcDTO
            {
                Code = pq.ProductCurrentValue!.Product!.Code,
                Name = pq.ProductCurrentValue!.Product!.Name,
                RequestedQuantity = pq.RequestedQuantity,
                Quoted01 = pq.Quoted01,
                Quoted02 = pq.Quoted02,
                Quoted03 = pq.Quoted03,
                QuotedValue = pq.QuotedValue,
                Statu = pq.Statu!.Name,
                StatuId = pq.StatuId,
                ValidityId = pq.ProductCurrentValue!.ValidityId,
                PriceHigh = pq.ProductCurrentValue.PriceHigh,
                PriceLow = pq.ProductCurrentValue.PriceLow,
            }).ToList();

            foreach (var item in results)
            {

                var filtered = response.Result!
                    .Where(pq => pq.ProductCurrentValue!.Product!.Code == item.Code)
                    .ToList();

                var ids = filtered
                                .Select(pq => pq.Id)
                                .ToList();

                item.Id = string.Join(",", ids);

                item.RequestedQuantity = filtered.Sum(pq => pq.RequestedQuantity);
            }

            return Ok(results);
        }

        return BadRequest(response.Message);
    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _productQuotationUnitOf.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] ProductQuotationHeadDTO entity)
    {
        var response = await _productQuotationUnitOf.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] ProductQuotationHeadDTO model)
    {
        var action = await _productQuotationUnitOf.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    //[HttpPut("fulle")]
    //public async Task<IActionResult> PustAsync(int id)
    //{
    //    var action = await _productQuotationUnitOf.UpdateAsync(id,0);

    //    if (action.WasSuccess)
    //    {
    //        return Ok(action.Result);
    //    }

    //    return BadRequest(action.Message);
    //}

    [HttpPut("fulls")]
    public async Task<IActionResult> PustAsync([FromBody] ProductQuotationPurcDTO model)
    {
        var action = !string.IsNullOrWhiteSpace(model.Name) ?
                            await _productQuotationUnitOf.UpdateAsync(model) :
                            model.Estado.Equals(true) ? await _productQuotationUnitOf.UpdateAsync(int.Parse(model.Id!), 0) :
                            await _productQuotationUnitOf.UpdateAsync(model.ValidityId);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }


    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _productQuotationUnitOf.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("ExistenRows/{id}/{lb}")]
    public async Task<IActionResult> GetAsync(int id, bool lb)
    {
        var response = await _productQuotationUnitOf.GetAsync(id, lb);

        if (response.WasSuccess)
        {

            return Ok(response.Result);
        }

        return BadRequest();
    }
}