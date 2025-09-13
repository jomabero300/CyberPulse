using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class BudgetProgramRepository : GenericRepository<BudgetProgram>, IBudgetProgramRepository
{
    private readonly ApplicationDbContext _context;
    public BudgetProgramRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<BudgetProgram>> GetAsync(int id)
    {
        var entity = await _context.BudgetPrograms
            .AsNoTracking()
            .Include(x => x.Program)
            .Include(x => x.Validity)
            .Include(x => x.Statu)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<BudgetProgram>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<BudgetProgram>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetPrograms
                                .AsNoTracking()
                                .Include(x => x.Program)
                                .Include(x => x.Budget)
                                .Include(x => x.Validity)
                                .Include(x => x.Statu)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Budget!.Rubro.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Validity!.Value.ToString().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<BudgetProgram>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Validity!.Value).ThenBy(y => y.Program!.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<BudgetProgram>> DeleteAsync(int id)
    {
        var entity = await _context.BudgetPrograms.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<BudgetProgram>> AddAsync(BudgetProgramDTO entity)
    {


        var model = new BudgetProgram
        {
            Id = entity.Id,
            BudgetId = entity.BudgetId,
            ProgramId = entity.ProgramId,
            BudgetTypeId = entity.BudgetTypeId,
            ValidityId = entity.ValidityId,
            Worth = entity.Worth,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<BudgetProgram>> GetComboAsync(int id)
    {
        return await _context.BudgetPrograms
            .AsNoTracking()
            .Where(x => x.Id == id)
            .OrderBy(x => x.ValidityId)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetPrograms
                                .Include(x => x.Program)
                                .Include(x => x.Budget)
                                .Include(x => x.Validity)
                                .Include(x => x.Statu)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Budget!.Rubro.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Validity!.Value.ToString().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<BudgetProgram>> UpdateAsync(BudgetProgramDTO entity)
    {
        var model = await _context.BudgetPrograms.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Id = entity.Id;
        model.BudgetId = entity.BudgetId;
        model.ProgramId = entity.ProgramId;
        model.BudgetTypeId = entity.BudgetTypeId;
        model.ValidityId = entity.ValidityId;
        model.Worth = entity.Worth;
        model.StatuId = entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetProgram>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
