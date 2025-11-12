using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ProductCurrentValueRepository : GenericRepository<ProductCurrentValue>, IProductCurrentValueRepository
{
    private readonly ApplicationDbContext _context;
    public ProductCurrentValueRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }



    public override async Task<ActionResponse<ProductCurrentValue>> GetAsync(int id)
    {
        var entity = await _context.ProductCurrentValues
            .AsNoTracking()
            .Include(x => x.Validity)
            .Include(x => x.Iva)
            .Include(x => x.Product)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<ProductCurrentValue>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<ProductCurrentValue>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.ProductCurrentValues
            .AsNoTracking()
            .Include(x => x.Validity)
            .Include(x => x.Iva)
            .Include(x => x.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Validity!.Value.ToString().ToLower().Contains(pagination.Filter.ToLower())||
                                             x.Iva!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<ProductCurrentValue>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Validity!.Value).ThenBy(x=>x.Product!.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<ProductCurrentValue>> DeleteAsync(int id)
    {
        var entity = await _context.ProductCurrentValues.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<ProductCurrentValue>> AddAsync(ProductCurrentValueDTO entity)
    {
        var model = new ProductCurrentValue
        {
            Id = entity.Id,
            ValidityId=entity.ValidityId,
            ProductId=entity.ProductId,
            IvaId=entity.IvaId,
            PriceLow=entity.PriceLow,
            PriceHigh=entity.PriceHigh,
            Worth =entity.Worth,
            Percentage=entity.Percentage,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<ProductCurrentValue>> GetComboAsync()
    {
        throw new NotImplementedException();
    }
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.ProductCurrentValues
                                .Include(x=>x.Validity)
                                .Include(x=>x.Iva)
                                .Include(x=>x.Product)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Validity!.Value.ToString().ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Iva!.Name.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task<ActionResponse<ProductCurrentValue>> GetAsync(double id)
    {
        var percentageParam = new SqlParameter
        {
            ParameterName = "@Percentage",
            SqlDbType = SqlDbType.Decimal,
            Direction = ParameterDirection.Input,
            Precision = 6, // Coincide con la definición (6,3)
            Scale = 3,
            Value = id
        };
        var errorCodeParam = new SqlParameter
        {
            ParameterName = "@ErrorCode",
            SqlDbType = SqlDbType.VarChar,
            Size = 6, // **Obligatorio para tipos de cadena de salida**
            Direction = ParameterDirection.Output
        };

        string sqlCommand = "EXEC Inve.ProductsWithNewValidity @Percentage, @ErrorCode OUTPUT";

        try
        {
            await _context.Database.ExecuteSqlRawAsync(sqlCommand, percentageParam, errorCodeParam);

            var errorCode = errorCodeParam.Value?.ToString() ?? string.Empty;

            if (errorCode == "Ok")
            {
                return new ActionResponse<ProductCurrentValue>
                {
                    WasSuccess = true,
                };
            }
            else
            {
                return new ActionResponse<ProductCurrentValue>
                {
                    WasSuccess = false,
                    Message = errorCode // Puedes mapear esto a un mensaje más amigable si es necesario
                };
            }
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }

        //var response = await _context.ProductCurrentValues.FromSqlRaw("EXECUTE Inve.ProductsWithNewValidity @Percentage, @ErrorCode OUTPUT", percentageParam, errorCodeParam).ToListAsync();

        //if (response == null)
        //{
        //    return new ActionResponse<ProductCurrentValue>
        //    {
        //        WasSuccess = true,
        //    };
        //}

        //return new ActionResponse<ProductCurrentValue>
        //{
        //    WasSuccess = false,
        //};
    }
    public async Task<ActionResponse<ProductCurrentValue>> UpdateAsync(ProductCurrentValueDTO entity)
    {
        var model = await _context.ProductCurrentValues.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.ValidityId = entity.ValidityId;
        model.ProductId= entity.ProductId;
        model.IvaId = entity.IvaId;
        model.PriceLow= entity.PriceLow;
        model.PriceHigh= entity.PriceHigh;
        model.Worth= entity.Worth;
        model.Percentage= entity.Percentage;


        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ProductCurrentValue>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}