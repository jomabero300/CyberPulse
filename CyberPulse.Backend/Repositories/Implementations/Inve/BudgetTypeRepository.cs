using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class BudgetTypeRepository : GenericRepository<BudgetType>, IBudgetTypeRepository
{
    private readonly ApplicationDbContext _context;
    public BudgetTypeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<BudgetType>> GetAsync(int id)
    {
        var entity = await _context.BudgetTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<BudgetType>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<BudgetType>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetTypes.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<BudgetType>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<BudgetType>> DeleteAsync(int id)
    {
        var entity = await _context.BudgetTypes.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetType>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<BudgetType>> AddAsync(BudgetTypeDTO entity)
    {
        var model = new BudgetType
        {
            Id = entity.Id,
            Name =HtmlUtilities.ToTitleCase( entity.Name.Trim().ToLower()),
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetType>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<BudgetType>> GetComboAsync()
    {
        return await _context.BudgetTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetTypes.AsQueryable();

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
    public async Task<ActionResponse<BudgetType>> UpdateAsync(BudgetTypeDTO entity)
    {
        var model = await _context.BudgetTypes.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower());

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetType>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetType>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}