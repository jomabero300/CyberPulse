using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ProductQuotationRepository : GenericRepository<ProductQuotation>, IProductQuotationRepository
{
    private readonly ApplicationDbContext _context;
    public ProductQuotationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<ProductQuotation>> GetAsync(int id)
    {
        var entity = await _context.ProductQuotations
            .AsNoTracking()
            .Include(x => x.BudgetCourse)
            .Include(x => x.ProductCurrentValue)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<ProductQuotation>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<ProductQuotation>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.ProductQuotations
            .AsNoTracking()
            .Include(x => x.BudgetCourse)
            .Include(x => x.ProductCurrentValue)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(pagination.Filter))
        //{
        //    queryable = queryable.Where(x =>
        //                                    x.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
        //                                    x.Classe.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
        //                                    x.Lot.Name.Contains(pagination.Filter.ToLower()));
        //}

        return new ActionResponse<IEnumerable<ProductQuotation>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Id)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<ProductQuotation>> DeleteAsync(int id)
    {
        var entity = await _context.ProductQuotations.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<ProductQuotation>> AddAsync(ProductQuotationDTO entity)
    {
        var model = new ProductQuotation
        {
            Id = entity.Id,
            BudgetCourseId =  entity.BudgetCourseId,
            ProductCurrentValueId = entity.ProductCurrentValueId,
            RequestedQuantity = entity.RequestedQuantity,
            AcceptedQuantity = entity.AcceptedQuantity,
            QuotedValue = entity.QuotedValue
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }


    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.ProductQuotations
            .AsNoTracking()
            .Include(x => x.BudgetCourse)
            .Include(x => x.ProductCurrentValue)
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(pagination.Filter))
        //{
        //    queryable = queryable.Where(x =>
        //                                    x.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
        //                                    x.Classe.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
        //                                    x.Lot.Name.ToLower().Contains(pagination.Filter.ToLower()));
        //}

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationDTO entity)
    {
        var model = await _context.ProductQuotations.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.BudgetCourseId = entity.BudgetCourseId;
        model.ProductCurrentValueId = entity.ProductCurrentValueId;
        model.RequestedQuantity = entity.RequestedQuantity;
        model.AcceptedQuantity = entity.AcceptedQuantity;
        model.QuotedValue = entity.QuotedValue;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}
