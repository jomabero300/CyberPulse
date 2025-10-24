using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class FamilyRepository : GenericRepository<Family>, IFamilyRepository
{
    private readonly ApplicationDbContext _context;
    public FamilyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<ActionResponse<Family>> GetAsync(int id)
    {
        var entity = await _context.Families
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Family>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Family>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Families
            .AsNoTracking()
            .Include(x => x.Segment)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()) || x.Code.ToString().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Family>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Family>> DeleteAsync(int id)
    {
        var entity = await _context.Families.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Family>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<Family>> AddAsync(FamilyDTO entity)
    {
        var model = new Family
        {
            Id = entity.Id,
            Code = entity.Code,
            Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
            SegmentId = entity.SegmentId,
            StatuId=entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Family>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Family>> GetComboAsync(int id)
    {
        return await _context.Families
            .AsNoTracking()
            .Where(f => f.SegmentId == id)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Families.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()) || x.Code.ToString().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<Family>> UpdateAsync(FamilyDTO entity)
    {
        var model = await _context.Families.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name =HtmlUtilities.ToTitleCase( entity.Name.Trim().ToLower());
        model.Code=entity.Code;
        model.StatuId=entity.StatuId;
        model.SegmentId=entity.SegmentId;


        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Family>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Family>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}