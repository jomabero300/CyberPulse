using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Product>> GetAsync(int id)
    {
        var entity = await _context.Products
            .AsNoTracking()
            .Include(x => x.Statu)
            .Include(x=>x.Classe).ThenInclude(x=>x.Family).AsNoTracking()
            .Include(x=>x.Lot)
            .Include(x=>x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Product>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Products
            .AsNoTracking()
            .Include(x => x.Statu)  
            .Include(x => x.Classe)  
            .Include(x => x.UnitMeasurement)  
            .Include(x => x.Lot)  
            .Include(x => x.Category)  
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                                            x.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.Code.ToString().Contains(pagination.Filter.ToLower()) ||
                                            x.Classe!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.Category!.Name!.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.Lot!.Name.Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Product>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<Product>> DeleteAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Product>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<Product>> AddAsync(ProductDTO entity)
    {
        var model = new Product
        {
            Id = entity.Id,
            Code=entity.Code,
            UnitMeasurementId=entity.UnitMeasurementId,
            CategoryId= entity.CategoryId,
            ClasseId =entity.ClasseId,
            LotId =entity.LotId,
            Name = HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
            Description = entity.Description.Trim(),
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Product>> GetComboAsync(int claseId, int lotId)
    {
        var queryable = _context.Products
            .AsNoTracking()
            .AsQueryable();

        if(lotId != 0)
        {
            queryable= queryable.Where(x=>x.LotId==lotId);
        }
        if(claseId != 0)
        {
            queryable= queryable.Where(x=>x.ClasseId==claseId);
        }

        return await queryable.OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Products
            .AsNoTracking()
            .Include(x => x.Statu)
            .Include(x => x.Classe)
            .Include(x => x.UnitMeasurement)
            .Include(x => x.Lot)
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                                            x.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.Code.ToString().Contains(pagination.Filter.ToLower()) ||
                                            x.Classe!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.Category!.Name!.ToLower().Contains(pagination.Filter.ToLower()) ||
                                            x.Lot!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<Product>> UpdateAsync(ProductDTO entity)
    {
        var model = await _context.Products.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name =HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower());
        model.Code= entity.Code;
        model.Description = entity.Description.Trim();
        model.LotId = entity.LotId;
        model.CategoryId = entity.CategoryId;
        model.ClasseId = entity.ClasseId;
        model.StatuId = entity.StatuId;
        model.UnitMeasurementId=entity.UnitMeasurementId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Product>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<IEnumerable<Product>> GetComboAsync()
    {
        return await _context.Products
                                .AsNoTracking()
                                .OrderBy(x=>x.Name)
                                .ToListAsync();
    }

    public async Task<ActionResponse<IEnumerable<Product>>> GetAsync(string Filter)
    {
        var queryable = _context.Products
                                .AsNoTracking()
                                .Include(P => P.UnitMeasurement)
                                .Include(P => P.Lot)
                                .Include(P => P.Classe)
                                .Include(P => P.Category)
                                .Include(P => P.Statu)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Filter) && Filter != "''")
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.Code.ToString().ToLower().Contains(Filter.ToLower()) ||
                                             x.Lot!.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.Classe!.Name.ToLower().Contains(Filter.ToLower()) ||
                                             x.Category!.Name!.ToLower().Contains(Filter.ToLower()) ||
                                             x.Statu!.Name.ToLower().Contains(Filter.ToLower()));
        }
        return new ActionResponse<IEnumerable<Product>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .ToListAsync()
        };

    }
}
