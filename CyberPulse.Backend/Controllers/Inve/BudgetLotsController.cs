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
public class BudgetLotsController : GenericController<BudgetLot>
{ 
    private readonly IBudgetLotUnitOfWork _budgetLotUnitOfWork;
    private readonly IBudgetCourseUnitOfWork _budgetCourseUnitOfWork;
    public BudgetLotsController(IGenericUnitOfWork<BudgetLot> unitOfWork, IBudgetLotUnitOfWork budgetLotUnitOfWork, IBudgetCourseUnitOfWork budgetCourseUnitOfWork) : base(unitOfWork)
    {
        _budgetLotUnitOfWork = budgetLotUnitOfWork;
        _budgetCourseUnitOfWork = budgetCourseUnitOfWork;
    }


    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _budgetLotUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _budgetLotUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            //BudgetLotIndexDTO
            var budgetTasks = response.Result!.Select(async x =>
            {
                var balanceResponse = await _budgetCourseUnitOfWork.GetBalanceAsync(x.Id);

                double useBalance = balanceResponse.WasSuccess ? balanceResponse.Result : 0.0;

                return new BudgetLotIndexDTO
                {
                    Id = x.Id,
                    BudgetProgramId= x.BudgetProgramId,
                    ProgramLotId= x.ProgramLotId,
                    Validity = x.Validity,
                    Worth = x.Worth,
                    StatuId = x.StatuId,
                    Balance = x.Worth - useBalance,
                    Statu = x.Statu,
                    BudgetProgram = x.BudgetProgram,
                    ProgramLot = x.ProgramLot,
                };
            });

            var budget = await Task.WhenAll(budgetTasks);

            return Ok(budget);
        }

        return BadRequest(response.Message);
    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _budgetLotUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] BudgetLotDTO entity)
    {
        var response = await _budgetLotUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] BudgetLotDTO model)
    {
        var action = await _budgetLotUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _budgetLotUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _budgetLotUnitOfWork.GetComboAsync(id));
    }
}