using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class InvProgramRepository : GenericRepository<InvProgram>, IInvProgramRepository
{
    private readonly ApplicationDbContext _context;
    public InvProgramRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<InvProgram>> GetAsync(int id)
    {
        var entity = await _context.InvPrograms
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<InvProgram>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<InvProgram>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.InvPrograms.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<InvProgram>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<InvProgram>> DeleteAsync(int id)
    {
        var entity = await _context.InvPrograms.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<InvProgram>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public async Task<ActionResponse<InvProgram>> AddAsync(InvProgramDTO entity)
    {
        var model = new InvProgram
        {
            Id = entity.Id,
            Name = entity.Name,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<InvProgram>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<InvProgram>> GetComboAsync()
    {
        return await _context.InvPrograms
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.InvPrograms.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<InvProgram>> UpdateAsync(InvProgramDTO entity)
    {
        var model = await _context.InvPrograms.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name = entity.Name;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<InvProgram>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<InvProgram>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
