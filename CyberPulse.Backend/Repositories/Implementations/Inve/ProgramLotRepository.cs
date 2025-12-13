using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ProgramLotRepository : GenericRepository<ProgramLot>, IProgramLotRepository
{
    private readonly ApplicationDbContext _context;
    public ProgramLotRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<ActionResponse<ProgramLot>> GetAsync(int id)
    {
        var entity = await _context.ProgramLots
            .AsNoTracking()
            .Include(x=>x.Program)
            .Include(x=>x.Lot)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<ProgramLot>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<ProgramLot>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.ProgramLots
                                .AsNoTracking()
                                .Include(x=>x.Program)
                                .Include(x=>x.Lot)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                                            x.Program!.Name.ToLower().Contains(pagination.Filter.ToLower())||
                                            x.Lot!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<ProgramLot>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Program!.Name).ThenBy(x=>x.Lot!.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<ProgramLot>> DeleteAsync(int id)
    {
        var entity = await _context.ProgramLots.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProgramLot>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<ProgramLot>> AddAsync(ProgramLotDTO entity)
    {
        var model = new ProgramLot
        {
            Id = entity.Id,
            LotId = entity.LotId,
            ProgramId = entity.ProgramId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProgramLot>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<ProgramLot>> GetComboAsync(int id)
    {
        return await _context.ProgramLots
                            .AsNoTracking()
                            .Include(x=>x.Lot)
                            .Where(x=>x.ProgramId==id)
                            .OrderBy(x => x.Program!.Name).ThenBy(l=>l.Lot!.Name)
                            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.ProgramLots.AsQueryable();

        //if (!string.IsNullOrWhiteSpace(pagination.Filter))
        //{
        //    queryable = queryable.Where(x => x.Rubro.ToLower().Contains(pagination.Filter.ToLower()));
        //}

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<IEnumerable<ProgramLot>>> GetAsync(string Filter)
    {
        var queryable = _context.ProgramLots
                                    .AsNoTracking()
                                    .Include(f => f.Program)
                                    .Include(f => f.Lot)
                                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Filter) && Filter != "''")
        {
            queryable = queryable.Where(x => x.Program!.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.Lot!.Name.ToString().ToLower().Contains(Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<ProgramLot>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Program!.Name).ThenBy(x => x.Lot!.Name)
                .ToListAsync()
        };
    }
    public async Task<ActionResponse<ProgramLot>> UpdateAsync(ProgramLotDTO entity)
    {
        var model = await _context.ProgramLots.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.ProgramId = entity.ProgramId;
        model.LotId = entity.LotId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProgramLot>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProgramLot>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}