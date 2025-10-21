using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class UnitMeasurementRepository : GenericRepository<UnitMeasurement>, IUnitMeasurementRepository
{
    private readonly ApplicationDbContext _context;
    public UnitMeasurementRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<ActionResponse<UnitMeasurement>> GetAsync(int id)
    {
        var entity = await _context.UnitMeasurements
            .AsNoTracking()
            .Include(x => x.Statu)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<UnitMeasurement>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<UnitMeasurement>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.UnitMeasurements
                                .AsNoTracking()
                                .Include(x => x.Statu)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<UnitMeasurement>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<UnitMeasurement>> DeleteAsync(int id)
    {
        var entity = await _context.UnitMeasurements.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<UnitMeasurement>> AddAsync(UnitMeasurementDTO entity)
    {
        var model = new UnitMeasurement
        {
            Id = entity.Id,
            Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
            Symbol=entity.Symbol.Trim().ToUpper(),
            BaseValue = entity.BaseValue,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<UnitMeasurement>> GetComboAsync()
    {
        return await _context.UnitMeasurements
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.UnitMeasurements.AsQueryable();

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
    public async Task<ActionResponse<UnitMeasurement>> UpdateAsync(UnitMeasurementDTO entity)
    {
        var model = await _context.UnitMeasurements.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower());
        model.Symbol =entity.Symbol.Trim().ToUpper();
        model.BaseValue = entity.BaseValue;
        model.StatuId = entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<UnitMeasurement>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
