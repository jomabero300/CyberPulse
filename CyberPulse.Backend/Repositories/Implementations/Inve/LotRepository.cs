using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using static CyberPulse.Backend.Repositories.Implementations.Inve.ProgramLotRepository;

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
            Name = HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
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

    //public async Task<IEnumerable<Lot>> GetComboAsync()
    //{
    //    return await _context.Lots
    //        .AsNoTracking()
    //        .OrderBy(x => x.Name)
    //        .ToListAsync();
    //}
    public async Task<IEnumerable<Lot>> GetComboAsync(int id,bool indEsta)
    {
        if (indEsta)
        {

            var assignedLotIds = await _context.ProgramLots.AsNoTracking()
                                             .Where(pl => pl.Id == id)
                                             .Select(pl => new {pl.ProgramId, pl.LotId })
                                             .FirstOrDefaultAsync();

            var result = _context.Lots.AsNoTracking()
                                             .Where(lot => lot.Id== assignedLotIds!.LotId || !_context.ProgramLots
                                                    .Where(pl=>pl.ProgramId== assignedLotIds.ProgramId)
                                                    .Select(pl=>pl.LotId)
                                                    .Contains(lot.Id))
                                            .AsQueryable();
            return await result.ToListAsync();

       }
        else
        {


            var assignedLotIds = _context.ProgramLots.AsNoTracking()
                                             .Where(pl => pl.ProgramId == id)
                                             .Select(pl => pl.LotId)
                                             .AsQueryable();

            var unassignedLots = _context.Lots.AsNoTracking()
                                             .Where(lot => !assignedLotIds.Contains(lot.Id))
                                             .AsQueryable();

            return await unassignedLots
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
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

        model.Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower());
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

    //public async Task<IEnumerable<Lot2DTO>> GetComboCourseAsync(int id)
    //{
    //    var query = await _context.BudgetLots
    //            .Where(bl => bl.Validity != null && bl.Validity.StatuId == 1)
    //            .Where(bl => bl.ProgramLot!.ProgramId == id)
    //            .Select(bl => new BudgetLotSaldoDto
    //            {
    //                ProgramLotId = bl.ProgramLotId,
    //                LotName = bl.ProgramLot!.Lot!.Name,
    //                BudgetLotWorth = bl.Worth,
    //                TotalBudgetCourseWorth = (from cpl in _context.CourseProgramLots
    //                                          where cpl.ProgramLotId == bl.ProgramLotId
    //                                          join bc in _context.BudgetCourses on cpl.Id equals bc.CourseProgramLotId
    //                                          select bc.Worth).Sum()
    //            })
    //            .ToListAsync();
    //    //TODO: VOY AQUI
    //    //var query = await (from bl in _context.BudgetLots.Include(bl => bl.Validity)
    //    //                   where bl.Validity!.StatuId == 1
    //    //                   join pl in _context.ProgramLots on bl.ProgramLotId equals pl.Id
    //    //                   where pl.ProgramId == id
    //    //                   join l in _context.Lots on pl.LotId equals l.Id
    //    //                   group new { bl, l } by new { pl.Id, l.Name, bl.Worth } into g
    //    //                   select new BudgetLotSaldoDto
    //    //                   {
    //    //                       ProgramLotId = g.Key.Id,
    //    //                       LotName = g.Key.Name,
    //    //                       BudgetLotWorth = g.Key.Worth,
    //    //                       TotalBudgetCourseWorth = (from cpl in _context.CourseProgramLots
    //    //                                                 where cpl.ProgramLotId == g.Key.Id
    //    //                                                 join bc in _context.BudgetCourses on cpl.Id equals bc.CourseProgramLotId
    //    //                                                 select bc.Worth).Sum() 
    //    //                   })
    //    //                   .ToListAsync();

    //    foreach (var item in query)
    //    {
    //        item.Saldo = item.BudgetLotWorth - item.TotalBudgetCourseWorth;
    //    }

    //    var entity = query.Select(q => new Lot2DTO
    //    {
    //        Id = q.ProgramLotId,
    //        Name = $"{q.LotName} - {q.Saldo.ToString("N2")}",
    //        Worth= q.Saldo
    //    }).ToList();

    //    return entity;
    //}

    private class BudgetLotSaldoDto
    {
        public int ProgramLotId { get; set; }
        public string LotName { get; set; }
        public double BudgetLotWorth { get; set; }
        public double TotalBudgetCourseWorth { get; set; }
        public double Saldo { get; set; } // BudgetLotWorth - TotalBudgetCourseWorth

        public int BudgetLotId { get; set; }
    }
}