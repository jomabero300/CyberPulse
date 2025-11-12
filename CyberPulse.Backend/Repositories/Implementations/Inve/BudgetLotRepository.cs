using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class BudgetLotRepository : GenericRepository<BudgetLot>, IBudgetLotRepository
{
    private readonly ApplicationDbContext _context;
    public BudgetLotRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<BudgetLot>> GetAsync(int id)
    {
        var entity = await _context.BudgetLots
            .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<BudgetLot>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<BudgetLot>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetLots.AsNoTracking()
            .Include(pl=>pl.ProgramLot).ThenInclude(x=>x!.Program)
            .Include(x => x.ProgramLot).ThenInclude(x=>x!.Lot)
            .Include(v=>v.BudgetProgram).ThenInclude(x=>x!.Validity)
            .Include(x=>x.Statu)
            .Where(w=>w.BudgetProgram!.Validity!.StatuId==1)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.ProgramLot!.Lot!.Name.Contains(pagination.Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<BudgetLot>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.BudgetProgram!.Program!.Name).ThenBy(Y=>Y.ProgramLot!.Lot!.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<BudgetLot>> DeleteAsync(int id)
    {
        var entity = await _context.BudgetLots.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        var budgetlot= await _context.BudgetLots.AsNoTracking().Where(x=>x.BudgetProgramId==entity.BudgetProgramId).ToListAsync();

        if(budgetlot !=null && budgetlot.Count==1)
        {
            var budgetProgram=await _context.BudgetPrograms.FindAsync(entity.BudgetProgramId);

            if(budgetProgram != null && budgetProgram.StatuId==11)
            {
                budgetProgram.StatuId=1;
                _context.Update(budgetProgram);
            }
        }
        
        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetLot>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public async Task<ActionResponse<BudgetLot>> AddAsync(BudgetLotDTO entity)
    {

        var validity=await _context.Validities.Where(x=>x.StatuId==1).FirstOrDefaultAsync();

        var model = new BudgetLot
        {
            Id = entity.Id,
            ValidityId = validity!.Id,
            BudgetProgramId = entity.BudgetProgramId,
            ProgramLotId=entity.ProgramLotId,
            Worth = entity.Worth,
            StatuId=entity.StatuId            
        };

        //var budgetProgram = await _context.BudgetPrograms.FindAsync(entity.BudgetProgramId);

        //if(budgetProgram != null && budgetProgram.StatuId == 1)
        //{
        //    budgetProgram.StatuId = 11;
        //    _context.Update(budgetProgram);
        //}

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetLot>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    //public async Task<IEnumerable<BudgetLot>> GetComboAsync()
    //{
    //    return await _context.BudgetLots
    //                         .AsNoTracking()
    //                         .ToListAsync();
    //}
    public async Task<IEnumerable<BudgetLot>> GetComboAsync(int id)
    {
        return await _context.BudgetLots
                             .AsNoTracking()
                             .Include(x=>x.ProgramLot).ThenInclude(x=>x!.Lot).ThenInclude(x=>x!.Statu)
                             .Where(x=>x.BudgetProgramId==id)
                             .Select(bl=>new BudgetLot
                             {
                                Id=bl.Id,
                                BudgetProgramId=bl.BudgetProgramId,
                                ProgramLotId =bl.ProgramLotId,
                                ValidityId=bl.ValidityId,
                                StatuId=bl.StatuId,
                                ProgramLot=bl.ProgramLot,
                                Worth=bl.Worth-(double)(_context.BudgetCourses
                                                                    .Where(bc=>bc.BudgetLotId==bl.Id)
                                                                    .Sum(bc=>(decimal?)bc.Worth)??0)
                             })
                             .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetLots.AsNoTracking()
                                .Include(pl => pl.ProgramLot).ThenInclude(x => x!.Program)
                                .Include(x => x.ProgramLot).ThenInclude(x => x!.Lot)
                                .Include(v => v.BudgetProgram).ThenInclude(x => x!.Validity)
                                .Include(x => x.Statu)
                                .Where(w => w.BudgetProgram!.Validity!.StatuId == 1)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.ProgramLot!.Lot!.Name.Contains(pagination.Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<BudgetLot>> UpdateAsync(BudgetLotDTO entity)
    {
        var model = await _context.BudgetLots.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.BudgetProgramId = entity.BudgetProgramId;
        model.ProgramLotId = entity.ProgramLotId;
        model.Worth = entity.Worth;
        model.StatuId=entity.StatuId;
        
        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetLot>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetLot>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
    public async Task<ActionResponse<double>> GetBalanceAsync(int id)
    {
        var balance = await _context.BudgetLots
                                    .AsNoTracking()
                                    .Include(x => x.Statu)
                                    .Where(x => x.BudgetProgramId == id)
                                    .SumAsync(x => (double)x.Worth);

        return new ActionResponse<double>
        {
            WasSuccess = true,
            Result = (double)balance
        };
    }
}
