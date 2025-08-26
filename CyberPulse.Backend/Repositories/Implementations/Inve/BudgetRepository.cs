using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
    private readonly ApplicationDbContext _context;
    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public override async Task<ActionResponse<IEnumerable<Budget>>> GetAsync()
    {
        var countries = await _context.Budgets
            .AsNoTracking()
            .ToListAsync();

        return new ActionResponse<IEnumerable<Budget>>
        {
            WasSuccess = true,
            Result = countries
        };

    }
    public override async Task<ActionResponse<Budget>> GetAsync(int id)
    {
        var country = await _context.Budgets
            .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);

        if (country == null)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Budget>
        {
            WasSuccess = true,
            Result = country
        };

    }
    public override async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Budgets.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Rubro.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Budget>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Rubro)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Budget>> DeleteAsync(int id)
    {
        var state = await _context.Budgets.FindAsync(id);

        if (state == null)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(state);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Budget>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public Task<ActionResponse<Budget>> AddAsync(BudgetDTO country)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Budget>> GetComboAsync()
    {
        return await _context.Budgets
            .AsNoTracking()
            .OrderBy(x => x.Rubro)
            .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Budgets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Rubro.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<Budget>> UpdateAsync(BudgetDTO entity)
    {
        var budgets = await _context.Budgets.FindAsync(entity.Id);

        if (budgets == null)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        budgets.Rubro = entity.Rubro;
        budgets.ValidityId = entity.ValidityId;
        budgets.BudgetTypeId = entity.BudgetTypeId;
        budgets.worth=entity.worth;

        _context.Update(budgets);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Budget>
            {
                WasSuccess = true,
                Result = budgets,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
