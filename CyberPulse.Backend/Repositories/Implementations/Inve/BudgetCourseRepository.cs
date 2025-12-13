using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                                   .AsNoTracking()
                                   .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                   .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                   .Include(bc => bc.Instructor)
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
        var queryable =string.IsNullOrWhiteSpace(pagination.Email) ?
                                _context.BudgetCourses.AsNoTracking()
                                              .Include(x=>x.Statu)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                              .Include(bc => bc.Instructor)
                                              .Where(bc => bc.Validity!.StatuId == 1 && bc.StatuId>=1 )
                                              .AsQueryable():
                                _context.BudgetCourses.AsNoTracking()
                                              .Include(x=>x.Statu)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                              .Include(bc => bc.Instructor)
                                              .Where(bc => bc.Validity!.StatuId == 1 && bc.Instructor.Email==pagination.Email && bc.StatuId >= 6)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.CourseProgramLot!.Course!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.CourseProgramLot!.ProgramLot!.Lot!.Name.Contains(pagination.Filter.ToLower()) ||
                                             x.Instructor!.FirstName.Contains(pagination.Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(pagination.Filter.ToLower()));
        }

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

        var budgetLot = await _context.BudgetLots.FindAsync(entity.BudgetLotId);

        if (budgetLot != null)
        {
            if ((budgetLot.Worth - entity.Worth) == 0)
            {
                budgetLot.StatuId = 1;
            }

            budgetLot.Worth = budgetLot.Worth - entity.Worth;

            _context.Update(budgetLot);
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


    public async Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(string id)
    {
        var entity = id == "0" ? await _context.BudgetCourses
                                                .AsNoTracking()
                                                .Include(cpl => cpl.CourseProgramLot).ThenInclude(c => c!.Course)
                                                .Include(bl => bl.BudgetLot).ThenInclude(pl => pl!.ProgramLot).ThenInclude(l => l!.Lot)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(p => p!.Program)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(b => b!.Budget).ThenInclude(s => s!.Statu)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(b => b!.Budget).ThenInclude(v => v!.Validity)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(b => b!.Budget).ThenInclude(bt => bt!.BudgetType)
                                                .Include(x => x.Validity)
                                                .Include(x => x.Statu)
                                                .ToListAsync() :
                              await _context.BudgetCourses
                                                .AsNoTracking()
                                                .Include(cpl => cpl.CourseProgramLot).ThenInclude(c => c!.Course)
                                                .Include(bl => bl.BudgetLot).ThenInclude(pl => pl!.ProgramLot).ThenInclude(l => l!.Lot)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(p => p!.Program)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(b => b!.Budget).ThenInclude(s => s!.Statu)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(b => b!.Budget).ThenInclude(v => v!.Validity)
                                                .Include(bl => bl.BudgetLot).ThenInclude(bp => bp!.BudgetProgram).ThenInclude(b => b!.Budget).ThenInclude(bt => bt!.BudgetType)
                                                .Include(x => x.Validity)
                                                .Include(x => x.Statu)
                                                .Where(x => x.ValidityId == int.Parse(id))
                                                .ToListAsync();

        return new ActionResponse<IEnumerable<BudgetCourse>>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public async Task<ActionResponse<IEnumerable<BudgetCourse>>> GetAsync(string Filter, bool estado)
    {
        var queryable = _context.BudgetCourses.AsNoTracking()
                                              .Include(x => x.Statu)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                              .Include(bc => bc.Instructor)
                                              .Where(bc => bc.Validity!.StatuId == 1 && bc.StatuId >= 1)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Filter) && Filter !="''")
        {
            queryable = queryable.Where(x => x.CourseProgramLot!.Course!.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.CourseProgramLot!.ProgramLot!.Lot!.Name.Contains(Filter.ToLower()) ||
                                             x.Instructor!.FirstName.Contains(Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<BudgetCourse>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.CourseProgramLot!.Course!.Name).ThenBy(y => y.StartDate)
                .ToListAsync()
        };
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
            InstructorId=entity.InstructorId,
            BudgetLotId = entity.BudgetLotId,
            ValidityId = Validity.Id,
            CourseProgramLotId = entity.CourseProgramLotId, 
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Worth = entity.Worth,
            StatuId = 1,
        };

        var budgetLotStatu = await _context.BudgetLots.FindAsync(entity.BudgetLotId);

        if (budgetLotStatu != null)
        {
            if( budgetLotStatu.StatuId == 1)
            {
                budgetLotStatu.StatuId = 11;
            }
            budgetLotStatu.Worth = budgetLotStatu.Worth+model.Worth;
            _context.Update(budgetLotStatu);
        }


        var budgetProgramStatu = await _context.BudgetPrograms.FindAsync(budgetLotStatu!.BudgetProgramId);

        if(budgetProgramStatu != null && budgetProgramStatu.StatuId == 1)
        {
            budgetProgramStatu.StatuId = 11;
            _context.Update(budgetProgramStatu);
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
        //var queryable = _context.BudgetCourses.AsNoTracking()
        //                                    .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
        //                                    .Where(bc => bc.Validity!.StatuId == 1 && bc.StatuId>1)
        //                                    .AsQueryable();

        var queryable = string.IsNullOrWhiteSpace(pagination.Email) ?
                                _context.BudgetCourses.AsNoTracking()
                                              .Include(x => x.Statu)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                              .Include(bc => bc.Instructor)
                                              .Where(bc => bc.Validity!.StatuId == 1 && bc.StatuId > 1)
                                              .AsQueryable() :
                                _context.BudgetCourses.AsNoTracking()
                                              .Include(x => x.Statu)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.Course)
                                              .Include(bc => bc.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                                              .Include(bc => bc.Instructor)
                                              .Where(bc => bc.Validity!.StatuId == 1 && bc.Instructor.Email == pagination.Email && bc.StatuId > 1)
                                              .AsQueryable();




        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.CourseProgramLot!.Course!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.CourseProgramLot!.ProgramLot!.Lot!.Name.Contains(pagination.Filter.ToLower()) ||
                                             x.Instructor!.FirstName.Contains(pagination.Filter.ToLower()) ||
                                             x.Worth.ToString().Contains(pagination.Filter.ToLower()));
        }

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

        if(entity.StatuId==7)
        {
            var productoQuatation = await _context.ProductQuotations.Where(x => x.BudgetCourseId == entity.Id).ToListAsync();
            if(!productoQuatation.Any())
            {
                return new ActionResponse<BudgetCourse>
                {
                    WasSuccess = false,
                    Message = "ERR019",
                };
            }
        }
        model.InstructorId = entity.InstructorId;
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