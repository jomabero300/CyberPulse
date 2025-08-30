using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class LotRepository : GenericRepository<Lot>, ILotRepository
{
    private readonly ApplicationDbContext _context;
    public LotRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }



    public override async Task<ActionResponse<Lot>> GetAsync(int id)
    {
        var entity = await _context.Lots
            .AsNoTracking()
            .Include(x=>x.Statu)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Lot>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Lot>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Lots.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Lot>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Lot>> DeleteAsync(int id)
    {
        var entity = await _context.Lots.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Lot>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }

    public async Task<ActionResponse<Lot>> AddAsync(LotDTO entity)
    {
        var model = new Lot
        {
            Id = entity.Id,
            Name = entity.Name,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Lot>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<IEnumerable<Lot>> GetComboAsync()
    {
        return await _context.Lots
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Lots.AsQueryable();

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

    public async Task<ActionResponse<Lot>> UpdateAsync(LotDTO entity)
    {
        var model = await _context.Lots.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name = entity.Name;
        model.StatuId=entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Lot>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Lot>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
