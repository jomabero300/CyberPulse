using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ClasseRepository : GenericRepository<Classe>, IClasseRepository
{
    private readonly ApplicationDbContext _context;
    public ClasseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Classe>> GetAsync(int id)
    {
        var entity = await _context.Classes
            .AsNoTracking()
            .Include(x=>x.Family)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Classe>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Classe>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Classes
            .AsNoTracking()
            .Include(x=>x.Family)
            .ThenInclude(s=>s.Segment)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Classe>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Classe>> DeleteAsync(int id)
    {
        var entity = await _context.Classes.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Classe>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<Classe>> AddAsync(ClasseDTO entity)
    {
        var model = new Classe
        {
            Id = entity.Id,
            Code=entity.Code,
            Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
            FamilyId = entity.FamilyId,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Classe>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Classe>> GetComboAsync(int id)
    {
        return await _context.Classes
            .AsNoTracking()
            .Where(x => x.FamilyId == id)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Classes.AsQueryable();

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
    public async Task<ActionResponse<Classe>> UpdateAsync(ClasseDTO entity)
    {
        var model = await _context.Classes.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name =HtmlUtilities.ToTitleCase( entity.Name.Trim().ToLower());
        model.Code= entity.Code;
        model.StatuId = entity.StatuId; 
        model.FamilyId=entity.FamilyId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Classe>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Classe>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
