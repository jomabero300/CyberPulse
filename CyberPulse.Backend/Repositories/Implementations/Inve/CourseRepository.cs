using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly ApplicationDbContext _context;
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Course>> GetAsync(int id)
    {
        var entity = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Course>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Course>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Courses.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Course>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Course>> DeleteAsync(int id)
    {
        var entity = await _context.Courses.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Course>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public async Task<ActionResponse<Course>> AddAsync(CourseDTO entity)
    {
        var model = new Course
        {
            Id = entity.Id,
            Name = entity.Name,
            StatuId=entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Course>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Course>> GetComboAsync(int id)
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(x=>x.LotId==id)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Courses.AsQueryable();

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

    public async Task<ActionResponse<Course>> UpdateAsync(CourseDTO entity)
    {
        var model = await _context.Courses.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name = entity.Name;
        model.LotId = entity.LotId;
        model.StatuId=entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Course>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Course>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
