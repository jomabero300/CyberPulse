using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.UnitsOfWork.Implementations.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Inve;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class BudgetCoursesController : GenericController<BudgetCourse>
{
    private readonly IBudgetCourseUnitOfWork _budgetCourseUnitOfWork;
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IMailHelper _mailHelper;

    public BudgetCoursesController(IGenericUnitOfWork<BudgetCourse> unitOfWork, IBudgetCourseUnitOfWork budgetCourseUnitOfWork, IUsersUnitOfWork usersUnitOfWork, IConfiguration configuration, IMailHelper mailHelper) : base(unitOfWork)
    {
        _budgetCourseUnitOfWork = budgetCourseUnitOfWork;
        _usersUnitOfWork = usersUnitOfWork;
        _configuration = configuration;
        _mailHelper = mailHelper;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _budgetCourseUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        if(!string.IsNullOrWhiteSpace(pagination.Email))
        {
            pagination.Email = User.Identity!.Name;
        }

        var response = await _budgetCourseUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _budgetCourseUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }



    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] BudgetCourseDTO entity)
    {
        var response = await _budgetCourseUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] BudgetCourseDTO model)
    {
        var action = await _budgetCourseUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpPut("fulls")]
    public async Task<IActionResult> PustAsync([FromBody] BudgetCourseSendDTO model)
    {
        var modelDto = new BudgetCourseDTO
                                        {
                                            Id = model.Id,
                                            InstructorId = model.InstructorId,
                                            BudgetLotId = model.BudgetLotId,
                                            ValidityId = model.ValidityId,
                                            CourseProgramLotId = model.CourseProgramLotId,
                                            StartDate = model.StartDate,
                                            EndDate = model.EndDate,
                                            Worth = model.Worth,
                                            StatuId = model.StatuId
                                        };

        var action = await _budgetCourseUnitOfWork.UpdateAsync(modelDto);

        if (action.WasSuccess)
        {
            if (model.StatuId == 6) //si el estado es 6 a enviado
            {
                //buscar el usuario e emails
                var user = await _usersUnitOfWork.GetUserAsync(model.InstructorId, UserType.Inst);

                var tokenLink = $"{HttpContext.Request.Scheme}://{_configuration["Url Frontend"]}";

                string Mailbody = model.StatuId switch
                {
                    6 => model.language == "es" ? "Mail:BodyCreateCourseEs" : "Mail:BodyCreateCourseEn",
                    _ => model.language == "es" ? "Mail:BodyReviewCourseEs" : "Mail:BodyReviewCourseEn",
                };

                string subject = model.language == "es" ? "Mail:SubjectCourseEs" : "Mail:SubjectCourseEn";

                await _mailHelper.SendMail(user.Result!.FullName, user.Result.Email!, _configuration[subject]!, string.Format(_configuration[Mailbody]!, model.Id.ToString(), tokenLink), model.language!);
            }
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        if (!string.IsNullOrWhiteSpace(pagination.Email))
        {
            pagination.Email = User.Identity!.Name;
        }

        var response = await _budgetCourseUnitOfWork.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

}
