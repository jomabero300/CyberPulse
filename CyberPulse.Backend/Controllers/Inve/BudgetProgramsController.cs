using CyberPulse.Backend.UnitsOfWork.Implementations.Inve;
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
public class BudgetProgramsController : GenericController<BudgetProgram>
{
    private readonly IBudgetProgramUnitOfWork _budgetProgramUnitOfWork;
    private readonly IBudgetLotUnitOfWork _budgetLotUnitOfWork;
    public BudgetProgramsController(IGenericUnitOfWork<BudgetProgram> unitOfWork, IBudgetProgramUnitOfWork budgetProgramUnitOfWork, IBudgetLotUnitOfWork budgetLotUnitOfWork) : base(unitOfWork)
    {
        _budgetProgramUnitOfWork = budgetProgramUnitOfWork;
        _budgetLotUnitOfWork = budgetLotUnitOfWork;
    }
    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _budgetProgramUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _budgetProgramUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            var budget = new List<BudgetProgramIndexDTO>();
            foreach (var x in response.Result!)
            {
                var balanceResponse = await _budgetLotUnitOfWork.GetBalanceAsync(x.Id);
                double usedBalance = balanceResponse.WasSuccess ? balanceResponse.Result : 0.0;

                budget.Add(new BudgetProgramIndexDTO
                {
                    Id = x.Id,
                    BudgetId = x.BudgetId,
                    ProgramId = x.ProgramId,
                    BudgetTypeId = x.BudgetTypeId,
                    ValidityId = x.ValidityId,
                    Worth = x.Worth,
                    StatuId = x.StatuId,
                    Balance = x.Worth - usedBalance,
                    Budget = x.Budget,
                    Program = x.Program,
                    BudgetType = x.BudgetType,
                    Validity = x.Validity,
                    Statu = x.Statu
                });
            }

            return Ok(budget);
        }

        return BadRequest(response.Message);
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _budgetProgramUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] BudgetProgramDTO entity)
    {
        var response = await _budgetProgramUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] BudgetProgramDTO model)
    {
        var action = await _budgetProgramUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _budgetProgramUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _budgetProgramUnitOfWork.GetComboAsync());
    }

}