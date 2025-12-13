using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
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
        var entity = await _context.Budgets
            .AsNoTracking()
            .ToListAsync();

        return new ActionResponse<IEnumerable<Budget>>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<Budget>> GetAsync(int id)
    {
        var entity = await _context.Budgets
            .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
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
            Result = entity
        };

    }
    public override async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Budgets.AsNoTracking()
                                        .Include(x => x.BudgetType)
                                        .Include(x => x.Validity)
                                        .Include(x => x.Statu)
                                        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Rubro.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.BudgetType!.Name.Contains(pagination.Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(pagination.Filter.ToLower()) ||
                                             x.Validity!.Value.ToString().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Budget>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.ValidityId).ThenBy(y => y.Rubro).ThenBy(z => z.BudgetTypeId)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Budget>> DeleteAsync(int id)
    {
        var entity = await _context.Budgets.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Budget>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

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

    public async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(string id)
    {
        var entity = await _context.Budgets.AsNoTracking()

                                        .Include(b => b.BudgetType)
                                        .Include(b => b.Statu)
                                        .Include(b => b.Validity)
                                        .ToListAsync();

        return new ActionResponse<IEnumerable<Budget>>
        {
            WasSuccess = true,
            Result = entity
        };
    }

    public async Task<ActionResponse<Budget>> AddAsync(BudgetDTO entity)
    {
        var budgetType = await _context.Budgets.AsNoTracking().Where(x => x.Rubro == entity.Rubro && x.ValidityId == entity.ValidityId).FirstOrDefaultAsync();

        if (budgetType != null)
        {
            entity.BudgetTypeId = 2;
        }

        var model = new Budget
        {
            Id = entity.Id,
            BudgetTypeId = entity.BudgetTypeId,
            Rubro = entity.Rubro,
            ValidityId = entity.ValidityId,
            Worth = entity.Worth,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Budget>
            {
                WasSuccess = true,
                Result = model
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
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Budget>> GetComboAsync()
    {
        return await _context.Budgets
            .AsNoTracking()
            .Include(x => x.Validity)
            .Include(x => x.BudgetType)
            .Where(x => x.Validity!.StatuId == 1)
            .Select(b => new Budget
            {
                Id = b.Id,
                BudgetType = b.BudgetType,
                BudgetTypeId = b.BudgetTypeId,
                ValidityId = b.ValidityId,
                Validity = b.Validity,
                Rubro = b.Rubro,
                Worth = b.Worth - (double)(_context.BudgetPrograms
                    .Where(bp => bp.BudgetId == b.Id)
                    .Sum(bp => (decimal?)bp.Worth) ?? 0)
            })
            .OrderBy(x => x.Rubro)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Budgets.AsNoTracking()
                    .Include(x => x.BudgetType)
                    .Include(x => x.Validity)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Rubro.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(pagination.Filter.ToLower()) ||
                                             x.BudgetType!.Name.Contains(pagination.Filter.ToLower()) ||
                                             x.Validity!.Value.ToString().Contains(pagination.Filter.ToLower()));
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
        budgets.Worth = entity.Worth;
        budgets.StatuId = entity.StatuId;

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

    public async Task<ActionResponse<IEnumerable<Budget>>> GetAsync(string Filter, bool Statu)
    {
        var queryable = _context.Budgets
                                .AsNoTracking()
                                .Include(b=>b.Validity)
                                .Include(b=>b.BudgetType)
                                .Include(f => f.Statu)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Filter) && Filter != "''")
        {
            queryable = queryable.Where(x => x.Rubro.ToLower().Contains(Filter.ToLower()) ||
                                             x.Validity!.Value.ToString().ToLower().Contains(Filter.ToLower()) ||
                                             x.BudgetType!.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.Worth.ToString().ToLower().Contains(Filter.ToLower()) ||
                                             x.Statu!.Name.ToLower().Contains(Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Budget>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.ValidityId).ThenBy(y => y.Rubro).ThenBy(z => z.BudgetTypeId)
                .ToListAsync()
        };

    }
}
