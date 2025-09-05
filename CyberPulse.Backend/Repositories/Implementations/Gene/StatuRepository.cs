using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Gene;

public class StatuRepository : GenericRepository<Statu>, IStatuRepository
{
    private readonly ApplicationDbContext _context;

    public StatuRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Statu>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Status.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        var resul = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Statu>>
        {
            WasSuccess = true,
            Result = resul,
        };

    }

    public async Task<ActionResponse<Statu>> GetAsync(string name, int nivel)
    {
        var response=await _context.Status.AsNoTracking().Where(x=>x.Name.ToLower()==name.ToLower() && x.Nivel==nivel).FirstOrDefaultAsync();
        
        if(response==null)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        return new ActionResponse<Statu>
        {
            WasSuccess=true,
            Result=response,
        };
    }

    //public override async Task<ActionResponse<Statu>> AddAsync(Statu entity)
    //{
    //    _context.Status.Add(entity);
    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //        return;

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public async Task<IEnumerable<Statu>> GetComboAsync(int id)
    {
        var query=  _context.Status.AsNoTracking().AsQueryable();

        if (id >= 0)
        {
            query = query.Where(x => x.Nivel == id);
        }


        return await query.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Status.AsNoTracking().AsQueryable();

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
}
