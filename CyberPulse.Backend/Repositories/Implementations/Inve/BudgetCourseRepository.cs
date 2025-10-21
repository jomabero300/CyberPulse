using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class BudgetCourseRepository : GenericRepository<BudgetCourse>, IBudgetCourseRepository
{
    private readonly ApplicationDbContext _context;
    public BudgetCourseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public override async Task<ActionResponse<BudgetCourse>> GetAsync(int id)
    {
        var entity = await _context.BudgetCourses
                                   .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                   .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<BudgetCourse>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetCourses.AsNoTracking()
            .Include(x=>x.Statu)
            .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
            .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
            .Where(w => w.Validity!.StatuId == 1)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(pagination.Filter))
        //{
        //    queryable = queryable.Where(x => x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
        //                                     x.ProgramLot!.Lot!.Name.Contains(pagination.Filter.ToLower()) ||
        //                                     x.Worth.ToString().Contains(pagination.Filter.ToLower()));
        //}

        return new ActionResponse<IEnumerable<BudgetCourse>>
        {
            WasSuccess = true,
            Result = await queryable
                .Paginate(pagination)
                .OrderBy(x=>x.CourseProgramLot!.Course!.Name).ThenBy(y=>y.StartDate)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<BudgetCourse>> DeleteAsync(int id)
    {
        var entity = await _context.BudgetCourses.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }



    public async Task<ActionResponse<BudgetCourse>> AddAsync(BudgetCourseDTO entity)
    {
        var Validity=await _context.Validities.Where(x=>x.StatuId==1).FirstOrDefaultAsync();

        if(Validity == null)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR014"
            };
        }

        var model = new BudgetCourse
        {
            Id = entity.Id,
            BudgetLotId = entity.BudgetLotId,
            ValidityId = Validity.Id,
            CourseProgramLotId = entity.CourseProgramLotId, 
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Worth = entity.Worth,
            StatuId = 1,
        };

        var budgetStatu = await _context.BudgetLots.FindAsync(entity.BudgetLotId);
        if (budgetStatu != null && budgetStatu.StatuId == 1)
        {
            budgetStatu.StatuId = 11;
            _context.Update(budgetStatu);
        }


        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<BudgetCourse>> GetComboAsync()
    {
        throw new NotImplementedException();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.BudgetCourses.AsNoTracking()
                                            .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                            .Where(w => w.Validity!.StatuId == 1)
                                            .AsQueryable();


        //if (!string.IsNullOrWhiteSpace(pagination.Filter))
        //{
        //    queryable = queryable.Where(x => x.ProgramLot!.Program!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
        //                                     x.ProgramLot!.Lot!.Name.Contains(pagination.Filter.ToLower()) ||
        //                                     x.Worth.ToString().Contains(pagination.Filter.ToLower()));
        //}

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<BudgetCourse>> UpdateAsync(BudgetCourseDTO entity)
    {
        var model = await _context.BudgetCourses.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.ValidityId = entity.ValidityId;
        model.CourseProgramLotId = entity.CourseProgramLotId;
        model.StartDate = entity.StartDate;
        model.EndDate = entity.EndDate;
        model.Worth = entity.Worth;
        model.StatuId = entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<BudgetCourse>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
    public async Task<ActionResponse<double>> GetBalanceAsync(int id)
    {
        var balance = await _context.BudgetCourses
                                    .AsNoTracking()
                                    .Include(x => x.Statu)
                                    .Where(x => x.BudgetLotId == id)
                                    .SumAsync(x => (double)x.Worth);

        return new ActionResponse<double>
        {
            WasSuccess = true,
            Result = (double)balance
        };
    }

}