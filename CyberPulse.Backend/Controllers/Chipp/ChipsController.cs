using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse.Backend.Controllers.Chipp;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ChipsController : GenericController<Chip>
{
    private readonly IChipUnitOfWork _chipUnitOfWork;
    private readonly ITypeOfPoblationUnitOfWork _poblationUnitOfWork;
    private readonly IChipRepository _chipRepository;
    private readonly IMailHelper _mailHelper;
    private readonly IConfiguration _configuration;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public ChipsController(IGenericUnitOfWork<Chip> unitOfWork, IChipUnitOfWork chipUnitOfWork, ITypeOfPoblationUnitOfWork poblationUnitOfWork, IChipRepository chipRepository, IMailHelper mailHelper, IConfiguration configuration, IUsersUnitOfWork usersUnitOfWork) : base(unitOfWork)
    {
        _chipUnitOfWork = chipUnitOfWork;
        _poblationUnitOfWork = poblationUnitOfWork;
        _chipRepository = chipRepository;
        _mailHelper = mailHelper;
        _configuration = configuration;
        _usersUnitOfWork = usersUnitOfWork;
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _chipUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {
            //buscar typeOfPoblation y llenar los que tenga TypeOfPoblationDTO
            List<TypeOfPoblationDTO> typeOfPoblationDTO = new();
            if (response.Result!.ChipPoblations != null)
            {
                var TypeOfPoblations = await _poblationUnitOfWork.GetAsync("Ninguno");

                foreach (var item in TypeOfPoblations)
                {
                    int quantity = 0;

                    var PoblationQuantity = response.Result.ChipPoblations.FirstOrDefault(x => x.TypePoblationId == item.Id);
                    if (PoblationQuantity != null)
                    {
                        quantity = PoblationQuantity.Quantity;
                    }

                    typeOfPoblationDTO.Add(new TypeOfPoblationDTO
                    {
                        Id = item.Id,
                        ChipDTOId = response.Result.Id,
                        Name = item.Name,
                        Quantity = quantity
                    });
                }
            }

            var result = new ChipDTO
            {
                Id = response.Result.Id,
                Apprentices = response.Result.Apprentices,
                ChipNo = response.Result.ChipNo,
                ChipProgramId = response.Result.ChipProgramId,
                Company = response.Result.Company,
                InstructorId = response.Result.InstructorId,
                StartDate = response.Result.StartDate,
                EndDate = response.Result.EndDate,
                AlertDate = response.Result.EndDate.AddDays(5),
                NeighborhoodId = response.Result.NeighborhoodId,
                TrainingProgramId = response.Result.TrainingProgramId,
                TypeOfTrainingId = response.Result.TypeOfTrainingId,
                UserId = response.Result.UserId,
                Duration = response.Result.ChipProgram.Duration,
                Justification = response.Result.Justification,
                WingMeasure = response.Result.ChipProgram.WingMeasure,

                MondayMorningStart = TimeSpan.Parse(response.Result.Monday.Substring(0, 5)),
                MondayMorningEnd = TimeSpan.Parse(response.Result.Monday.Substring(6, 5)),
                MondayAfternoonStart = TimeSpan.Parse(response.Result.Monday.Substring(12, 5)),
                MondayAfternoonEnd = TimeSpan.Parse(response.Result.Monday.Substring(18, 5)),

                TuesdayMorningStart = TimeSpan.Parse(response.Result.Tuesday.Substring(0, 5)),
                TuesdayMorningEnd = TimeSpan.Parse(response.Result.Tuesday.Substring(6, 5)),
                TuesdayAfternoonStart = TimeSpan.Parse(response.Result.Tuesday.Substring(12, 5)),
                TuesdayAfternoonEnd = TimeSpan.Parse(response.Result.Tuesday.Substring(18, 5)),

                WednesdayMorningStart = TimeSpan.Parse(response.Result.Wednesday.Substring(0, 5)),
                WednesdayMorningEnd = TimeSpan.Parse(response.Result.Wednesday.Substring(6, 5)),
                WednesdayAfternoonStart = TimeSpan.Parse(response.Result.Wednesday.Substring(12, 5)),
                WednesdayAfternoonEnd = TimeSpan.Parse(response.Result.Wednesday.Substring(18, 5)),

                TursdayMorningStart = TimeSpan.Parse(response.Result.Tursday.Substring(0, 5)),
                TursdayMorningEnd = TimeSpan.Parse(response.Result.Tursday.Substring(6, 5)),
                TursdayAfternoonStart = TimeSpan.Parse(response.Result.Tursday.Substring(12, 5)),
                TursdayAfternoonEnd = TimeSpan.Parse(response.Result.Tursday.Substring(18, 5)),

                FridayMorningStart = TimeSpan.Parse(response.Result.Friday.Substring(0, 5)),
                FridayMorningEnd = TimeSpan.Parse(response.Result.Friday.Substring(6, 5)),
                FridayAfternoonStart = TimeSpan.Parse(response.Result.Friday.Substring(12, 5)),
                FridayAfternoonEnd = TimeSpan.Parse(response.Result.Friday.Substring(18, 5)),

                SaturdayMorningStart = TimeSpan.Parse(response.Result.Saturday.Substring(0, 5)),
                SaturdayMorningEnd = TimeSpan.Parse(response.Result.Saturday.Substring(6, 5)),
                SaturdayAfternoonStart = TimeSpan.Parse(response.Result.Saturday.Substring(12, 5)),
                SaturdayAfternoonEnd = TimeSpan.Parse(response.Result.Saturday.Substring(18, 5)),

                SundayMorningStart = TimeSpan.Parse(response.Result.Sunday.Substring(0, 5)),
                SundayMorningEnd = TimeSpan.Parse(response.Result.Sunday.Substring(6, 5)),
                SundayAfternoonStart = TimeSpan.Parse(response.Result.Sunday.Substring(12, 5)),
                SundayAfternoonEnd = TimeSpan.Parse(response.Result.Sunday.Substring(18, 5)),

                StatuId = response.Result.StatuId,
                idEsta = response.Result.idEsta,

                TypeOfPoblationDTO = typeOfPoblationDTO.ToList()
            };

            return Ok(result);
        }

        return BadRequest();
    }
    [HttpGet("full")]
    public async Task<IActionResult> GetAsync(int id, bool indEsta)
    {
        var response = await _chipUnitOfWork.GetAsync(id);

        if (response.WasSuccess)
        {

            var result = new ChipCoordinator
            {
                Id = response.Result.Id,
                ChipNo = response.Result.ChipNo,
                ChipProgramId = response.Result.ChipProgramId,
                InstructorId = response.Result.InstructorId,
                StatuId = response.Result.StatuId,
                ChipProgramName = response.Result.ChipProgram.Designation,
                Code = response.Result.ChipProgram.Code,
                Identificacion = response.Result.Instructor.DocumentId,
                InstructorName = response.Result.Instructor.FullName,
                StartDate = response.Result.StartDate,
                idEsta = response.Result.idEsta
            };

            return Ok(result);
        }

        return BadRequest();
    }


    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _chipRepository.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _chipUnitOfWork.DeleteAsync(id);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();

    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] ChipDTO entity)
    {
        var response = await _chipUnitOfWork.AddAsync(entity);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] ChipDTO model)
    {
        var action = await _chipUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpPut("fullc")]
    public async Task<IActionResult> PustAsync([FromBody] ChipCoordinator model)
    {
        var action = await _chipUnitOfWork.UpdateAsync(model);

        if (action.WasSuccess)
        {
            if (model.Code == "E")
            {
                //buscar el usuario e emails
                var user = await _usersUnitOfWork.GetUserAsync(model.InstructorId, UserType.Inst);
                var tokenLink = "https://localhost:7244";
                //TODO: ACTIVAR ENVIAR EMAIL, ESTA DESACTIVADO PARA HACER PRUEBAS
                //_mailHelper.SendMail(user.Result!.FullName, user.Result.Email!, _configuration["Mail:SubjectCreateChipEs"]!, string.Format(_configuration["Mail:BodyCreateChipEs"]!, tokenLink), "es");
            }

            return Ok(action.Result);
        }

        return BadRequest(action.Message);
    }

    [HttpGet("TotalRecordsPaginated")]
    public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _chipRepository.GetTotalRecordsAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }
}
