using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class ChipRepository : GenericRepository<Chip>, IChipRepository
{
    private readonly ApplicationDbContext _context;
    public ChipRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Chip>> GetAsync(int id)
    {
        var entity = await _context.Chips
            .Include(x => x.ChipPoblations)
            .Include(x => x.ChipProgram)
            .Where(x => x.Id == id).FirstOrDefaultAsync();

        if (entity == null)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Chip>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<Chip>> DeleteAsync(int id)
    {
        var entity = await _context.Chips.FindAsync(id);
        if (entity == null)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        var entityDetails = await _context.ChipPoblations.Where(x => x.ChipId == id).ToListAsync();
        _context.RemoveRange(entityDetails);
        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Chip>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }
    public override async Task<ActionResponse<IEnumerable<Chip>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Chips.Include(x => x.ChipProgram).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.ChipNo.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.ChipProgram.Designation.ToLower().Contains(pagination.Filter.ToLower()));
        }

        var resul = await queryable
            .OrderBy(x => x.ChipNo)
            .Paginate(pagination)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Chip>>
        {
            WasSuccess = true,
            Result = resul,
        };
    }


    public async Task<ActionResponse<Chip>> AddAsync(ChipDTO entity)
    {

        string monday = HoraCadena(entity.MondayMorningStar, entity.MondayMorningEnd, entity.MondayAfternoonStar, entity.MondayAfternoonEnd);
        string tuesday = HoraCadena(entity.TuesdayMorningStar, entity.TuesdayMorningEnd, entity.TuesdayAfternoonStar, entity.TuesdayAfternoonEnd);
        string wednesday = HoraCadena(entity.WednesdayMorningStar, entity.WednesdayMorningEnd, entity.WednesdayAfternoonStar, entity.WednesdayAfternoonEnd);
        string tursday = HoraCadena(entity.TursdayMorningStar, entity.TursdayMorningEnd, entity.TursdayAfternoonStar, entity.TursdayAfternoonEnd);
        string friday = HoraCadena(entity.FridayMorningStar, entity.FridayMorningEnd, entity.FridayAfternoonStar, entity.FridayAfternoonEnd);
        string saturday = HoraCadena(entity.SaturdayMorningStar, entity.SaturdayMorningEnd, entity.SaturdayAfternoonStar, entity.SaturdayAfternoonEnd);
        string sunday = HoraCadena(entity.SundayMorningStar, entity.SundayMorningEnd, entity.SundayAfternoonStar, entity.SundayAfternoonEnd);


        var chipPoblations1 = entity.TypeOfPoblationDTO.Where(x => x.Quantity != 0).ToList();

        var chipPoblations = chipPoblations1.Select(x => new ChipPoblation
        {
            TypePoblationId = x.Id,
            ChipId = x.ChipDTOId,
            Quantity = x.Quantity
        });


        var chip = new Chip()
        {
            Apprentices = entity.Apprentices,
            ChipNo = entity.ChipNo,
            ChipProgramId = entity.ChipProgramId,
            Company = entity.Company,
            InstructorId = entity.InstructorId,
            EndDate = entity.EndDate,
            NeighborhoodId = entity.NeighborhoodId,
            TrainingProgramId = entity.TrainingProgramId,
            TypeOfTrainingId = entity.TypeOfTrainingId,
            UserId = entity.UserId,
            Justification = entity.Justification,
            Monday=monday,
            Tuesday = tuesday,
            Wednesday = wednesday,
            Tursday = tursday,
            Friday = friday,
            Saturday = saturday,
            Sunday = sunday,
            ChipPoblations = chipPoblations.ToList(),
        };

        _context.Add(chip);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Chip>
            {
                WasSuccess = true,
                Result = chip
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Chips.AsQueryable();

        //if (!string.IsNullOrWhiteSpace(pagination.Filter))
        //{
        //    queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        //}

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count,
        };
    }
    public async Task<ActionResponse<Chip>> UpdateAsync(ChipDTO entity)
    {
        var chip = await _context.Chips.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

        if (chip == null)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        string monday = HoraCadena(entity.MondayMorningStar, entity.MondayMorningEnd, entity.MondayAfternoonStar, entity.MondayAfternoonEnd);
        string tuesday = HoraCadena(entity.TuesdayMorningStar, entity.TuesdayMorningEnd, entity.TuesdayAfternoonStar, entity.TuesdayAfternoonEnd);
        string wednesday = HoraCadena(entity.WednesdayMorningStar, entity.WednesdayMorningEnd, entity.WednesdayAfternoonStar, entity.WednesdayAfternoonEnd);
        string tursday = HoraCadena(entity.TursdayMorningStar, entity.TursdayMorningEnd, entity.TursdayAfternoonStar, entity.TursdayAfternoonEnd);
        string friday = HoraCadena(entity.FridayMorningStar, entity.FridayMorningEnd, entity.FridayAfternoonStar, entity.FridayAfternoonEnd);
        string saturday = HoraCadena(entity.SaturdayMorningStar, entity.SaturdayMorningEnd, entity.SaturdayAfternoonStar, entity.SaturdayAfternoonEnd);
        string sunday = HoraCadena(entity.SundayMorningStar, entity.SundayMorningEnd, entity.SundayAfternoonStar, entity.SundayAfternoonEnd);

        chip.Apprentices = entity.Apprentices;
        chip.ChipNo = entity.ChipNo;
        chip.ChipProgramId = entity.ChipProgramId;
        chip.Company = entity.Company;
        chip.InstructorId = entity.InstructorId;
        chip.EndDate = entity.EndDate;
        chip.NeighborhoodId = entity.NeighborhoodId;
        chip.TrainingProgramId = entity.TrainingProgramId;
        chip.TypeOfTrainingId = entity.TypeOfTrainingId;
        chip.UserId = entity.UserId;
        chip.Justification = entity.Justification;
        chip.Monday = monday;
        chip.Tuesday = tuesday;
        chip.Wednesday = wednesday;
        chip.Tursday = tursday;
        chip.Friday = friday;
        chip.Saturday = saturday;
        chip.Sunday = sunday;

        var chipPoblations1 = entity.TypeOfPoblationDTO.Where(x => x.Quantity != 0).ToList();

        var chipPoblations = chipPoblations1.Select(x => new ChipPoblation
        {
            TypePoblationId = x.Id,
            ChipId = x.ChipDTOId,
            Quantity = x.Quantity
        });


        try
        {
            await _context.ChipPoblations.Where(x => x.ChipId == entity.Id).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }

        _context.AddRange(chipPoblations);

        try
        {
            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }


        _context.Update(chip);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Chip>
            {
                WasSuccess = true,
                Result = chip,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Chip>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }

    }

    private string HoraCadena(TimeSpan MorningStar, TimeSpan MorningEnd, TimeSpan AfternoonStar, TimeSpan AfternoonEnd)
    {
        string Morning = "00:00-00:00";
        string Afternoon = "00:00-00:00";

        if (MorningStar > TimeSpan.Zero && MorningEnd > MorningStar)
        {
            Morning = $"{MorningStar.ToString(@"hh\:mm")}-{MorningEnd.ToString(@"hh\:mm")}";
        }

        if (AfternoonStar > TimeSpan.Zero && AfternoonEnd > AfternoonStar)
        {
            Afternoon = $"{AfternoonStar.ToString(@"hh\:mm")}-{AfternoonEnd.ToString(@"hh\:mm")}";
        }
        return $"{Morning}-{Afternoon}";
    }
}
