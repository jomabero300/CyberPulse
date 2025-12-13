using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class SegmentRepository : GenericRepository<Segment>, ISegmentRepository
{
    private readonly ApplicationDbContext _context;
    public SegmentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Segment>> GetAsync(int id)
    {
        var entity = await _context.Segments
            .AsNoTracking()
            .Include(x => x.Statu)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Segment>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Segment>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Segments.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower())|| 
                                             x.Code.ToString().ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Segment>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Segment>> DeleteAsync(int id)
    {
        var entity = await _context.Segments.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Segment>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }

                                               
    public async Task<ActionResponse<IEnumerable<Segment>>> GetAsync(string Filter)
    {
        var queryable = _context.Segments.AsNoTracking().Include(s=>s.Statu)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Filter) && Filter!="''")
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.Code.ToString().ToLower().Contains(Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Segment>>
        {
            WasSuccess = true,
            Result = await queryable
                            .OrderBy(x => x.Name)
                            .ToListAsync()
        };
    }

    public async Task<ActionResponse<Segment>> AddAsync(SegmentDTO entity)
    {
        var model = new Segment
        {
            Id = entity.Id,
            Code= entity.Code,
            Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Segment>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Segment>> GetComboAsync()
    {
        return await _context.Segments
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Segments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Code.ToString().ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<Segment>> UpdateAsync(SegmentDTO entity)
    {
        var model = await _context.Segments.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower());
        model.Code=entity.Code;
        model.StatuId = entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Segment>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Segment>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}