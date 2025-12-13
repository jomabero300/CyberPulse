using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Category>> GetAsync(int id)
    {
        var entity = await _context.Categories
                                    .AsNoTracking()
                                    .Include(x => x.Statu)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Category>
        {
            WasSuccess = true,
            Result = entity
        };

    }
    public override async Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Categories
                                    .AsNoTracking()
                                    .Include(x=>x.Statu)
                                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Statu!.Name.Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Category>>
        {
            WasSuccess = true,
            Result = await queryable
                            .OrderBy(x => x.Name)
                            .Paginate(pagination)
                            .ToListAsync()
        };

    }
    public override async Task<ActionResponse<Category>> DeleteAsync(int id)
    {
        var entity = await _context.Categories
                                    .AsNoTracking()
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();

        if (entity == null)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }
        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Category>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }

    public async Task<ActionResponse<Category>> AddAsync(CategoryDTO entity)
    {
        var model=new Category()
        {
            Name=entity.Name,
            Description=entity.Description,
            StatuId=entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Category>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<IEnumerable<Category>> GetComboAsync()
    {
        return await _context.Categories.AsNoTracking()
                                        .OrderBy(x => x.Name)
                                        .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Categories
                                    .AsNoTracking()
                                    .Include(x => x.Statu)
                                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Statu!.Name.Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };

    }

    public async Task<ActionResponse<Category>> UpdateAsync(CategoryDTO entity)
    {
        var model = await _context.Categories.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name=entity.Name;
        model.Description=entity.Description;
        model.StatuId = entity.StatuId;

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Category>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Category>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
}
