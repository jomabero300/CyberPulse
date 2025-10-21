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
public class BudgetsController : GenericController<Budget>
{
    private readonly IBudgetUnitOfWork _budgetUnitOf;
    private readonly IBudgetProgramUnitOfWork _budgetProgramUnitOf;

    public BudgetsController(IGenericUnitOfWork<Budget> unitOfWork, IBudgetUnitOfWork budgetUnitOf, IBudgetProgramUnitOfWork budgetProgramUnitOf) : base(unitOfWork)
    {
        _budgetUnitOf = budgetUnitOf;
        _budgetProgramUnitOf = budgetProgramUnitOf;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _budgetUnitOf.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _budgetUnitOf.GetAsync(pagination);

        if (response.WasSuccess)
        {

            var budget=new List<Budget1DTO>();

            foreach (var item in response.Result!)
            {
                var balanceResponse = await _budgetProgramUnitOf.GetBalanceAsync(item.Id);

                double useBalance = balanceResponse.WasSuccess ? balanceResponse.Result : 0.0;

                budget.Add(new Budget1DTO
                {
                    Id = item.Id,
                    BudgetType = item.BudgetType,
                    BudgetTypeId = item.BudgetTypeId,
                    Validity = item.Validity,
                    ValidityId = item.ValidityId,
                    Rubro = item.Rubro,
                    Worth = item.Worth,
                    Balance = item.Worth - useBalance,
                    Statu = item.Statu,
                    StatuId = item.StatuId

                });
            }


            //var budgetTasks = response.Result!.Select(async x =>
            //{
            //    var balanceResponse = await _budgetProgramUnitOf.GetBalanceAsync(x.Id);

            //    double useBalance = balanceResponse.WasSuccess ? balanceResponse.Result : 0.0;

            //    return new Budget1DTO
            //    {
            //        Id = x.Id,
            //        BudgetType = x.BudgetType,
            //        BudgetTypeId = x.BudgetTypeId,
            //        Validity = x.Validity,
            //        ValidityId = x.ValidityId,
            //        Rubro = x.Rubro,
            //        Worth = x.Worth,
            //        Balance = x.Worth - useBalance,
            //        Statu = x.Statu,
            //        StatuId = x.StatuId
            //    };
            //});

            //var budget = await Task.WhenAll(budgetTasks);

            return Ok(budget);
        }

        return BadRequest();
    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _budgetUnitOf.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] BudgetDTO entity)
    {
        var response = await _budgetUnitOf.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] BudgetDTO model)
    {
        var action = await _budgetUnitOf.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _budgetUnitOf.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _budgetUnitOf.GetComboAsync());
    }
}
