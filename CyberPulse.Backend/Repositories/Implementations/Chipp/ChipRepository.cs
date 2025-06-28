using CyberPulse.Backend.Data;
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

    public async Task<ActionResponse<Chip>> AddAsync(ChipDTO entity)
    {
        var chip = new Chip()
        {
            Apprentices = entity.Apprentices,
            ChipNo = entity.ChipNo,
            ChipProgramId = entity.ChipProgramId,
            Company = entity.Company,
            InstructorId = entity.InstructorId,
            EndDate = entity.EndDate,
            NeighborhoodId = entity.NeighborhoodId,
            TypeOfTrainingId = entity.TypeOfTrainingId,
            UserId = entity.UserId,
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

    public Task<ActionResponse<Chip>> UpdateAsync(ChipDTO entity)
    {
        throw new NotImplementedException();
    }
}
