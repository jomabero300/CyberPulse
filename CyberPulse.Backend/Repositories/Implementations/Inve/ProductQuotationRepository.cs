using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.Vml.Office;
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
                                    .Include(x => x.ProductCurrentValue).ThenInclude(x => x!.Product)
                                    .Include(x => x.Statu)
                                .Where(x => x.ProductCurrentValue!.Validity!.Statu!.Name == "Activo" && x.BudgetCourse!.StatuId>6)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x =>
                                            x.ProductCurrentValue!.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }


        return new ActionResponse<IEnumerable<ProductQuotation>>
        {
            WasSuccess = true,
            Result = await queryable
                .Paginate(pagination)
                .OrderBy(bc => bc.Id)
                .ToListAsync()
        };
    }
    public override async Task<ActionResponse<ProductQuotation>> DeleteAsync(int id)
    {
        var entity = await _context.ProductQuotations.Where(x=>x.BudgetCourseId==id).ToListAsync();

        if (entity == null)
        {
            return new ActionResponse<ProductQuotation>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.RemoveRange(entity);

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
            QuotedValue = entity.QuotedValue,
            Quoted01= entity.Quoted01,
            Quoted02= entity.Quoted02,
            Quoted03= entity.Quoted03,
            StatuId=entity.StatuId
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
    public async Task<ActionResponse<List<ProductQuotation>>> AddAsync(ProductQuotationHeadDTO entity)
    {
        var modelNew = entity.ProductQuotationBody!
            .Select(x => new ProductQuotation
            {
                Id = x.Id,
                BudgetCourseId = x.BudgetCourseId,
                ProductCurrentValueId = x.ProductCurrentValueId,
                RequestedQuantity = x.RequestedQuantity,
                AcceptedQuantity = x.AcceptedQuantity,
                QuotedValue = x.QuotedValue,
                Quoted01=x.Quoted01,
                Quoted02=x.Quoted02,
                Quoted03=x.Quoted03,
                StatuId=x.StatuId
            })
            .Where(x=>x.Id==0 && x.RequestedQuantity>0)
            .ToList();
        if(modelNew.Count>0)
        {
            _context.AddRange(modelNew);
        }

        var modelUpdate = entity.ProductQuotationBody!
            .Select(x => new ProductQuotation
            {
                Id = x.Id,
                BudgetCourseId = x.BudgetCourseId,
                ProductCurrentValueId = x.ProductCurrentValueId,
                RequestedQuantity = x.RequestedQuantity,
                AcceptedQuantity = x.AcceptedQuantity,
                QuotedValue = x.QuotedValue,
                Quoted01 = x.Quoted01,
                Quoted02 = x.Quoted02,
                Quoted03 = x.Quoted03,
                StatuId = x.StatuId
            })
            .Where(x => x.Id != 0)
            .ToList();
        if(modelUpdate.Count>0)
        {
            _context.UpdateRange(modelUpdate);
        }


        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<List<ProductQuotation>>
            {
                WasSuccess = true,
                Result = modelNew
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<List<ProductQuotation>>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<List<ProductQuotation>>
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
                                    .Include(x => x.ProductCurrentValue).ThenInclude(x=>x!.Product)
                                    .Include(x=>x.Statu)
                                .Where(bc=>bc.ProductCurrentValue!.Validity!.Statu!.Name=="Activo" && bc.BudgetCourse!.StatuId>6)
                                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x =>
                                            x.ProductCurrentValue!.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        //int count = await queryable.GroupBy(bc => bc.ProductCurrentValue!.Product!.Code).CountAsync();
        int count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = count
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
        model.Quoted01= entity.Quoted01;
        model.Quoted02= entity.Quoted02;
        model.Quoted03= entity.Quoted03;
        model.StatuId=entity.StatuId;

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
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationPurcDTO entity)
    {
        var ids =entity.Id!.Split(',')
           .Select(s => int.Parse(s.Trim()))
           .ToArray();

        await _context.ProductQuotations
                                        .Where(x => ids.Contains(x.Id))
                                        .ExecuteUpdateAsync(y => y
                                                .SetProperty(q => q.Quoted01, entity.Quoted01)
                                                .SetProperty(q => q.Quoted02, entity.Quoted02)
                                                .SetProperty(q => q.Quoted03, entity.Quoted03)
                                                .SetProperty(q => q.QuotedValue, entity.QuotedValue));
        return new ActionResponse<ProductQuotation>
        {
            WasSuccess = true
        };
    }
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(ProductQuotationHeadDTO entity)
    {
        var result = await _context.ProductQuotations
                                        .Where(x => x.BudgetCourseId == entity.Id).ToListAsync();

        foreach (var item in result)
        {
            var row = entity.ProductQuotationBody!.FirstOrDefault(x => x.Id == item.Id);

            if (row != null)
            {
                item.AcceptedQuantity = row!.RequestedQuantity;
            }
        }

        await _context.SaveChangesAsync();

        return new ActionResponse<ProductQuotation>
        {
            WasSuccess = true
        };

    }
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(int id, int esta)
    {
        await _context.ProductQuotations
                                    .Where(x => x.BudgetCourse!.Id == id)
                                    .ExecuteUpdateAsync(y => y.SetProperty(q => q.StatuId, 11));

        var results = await _context.BudgetCourses
                                         .Where(bc => bc.ProductQuotations!.Any())
                                         .Where(bc => bc.Id == id && bc.ProductQuotations!
                                                .All(pq => pq.QuotedValue != 0))
                                         .ExecuteUpdateAsync(y => y.SetProperty(bc => bc.StatuId, 11));

        return new ActionResponse<ProductQuotation>
        {
            WasSuccess = true
        };
    }
    public async Task<ActionResponse<ProductQuotation>> UpdateAsync(int id)
    {
        await _context.ProductQuotations
                                    .Where(x => x.BudgetCourse!.ValidityId == id && x.QuotedValue != 0)
                                    .ExecuteUpdateAsync(y => y.SetProperty(q => q.StatuId, 8));

        var results = await _context.BudgetCourses
                                             .Where(bc => bc.ProductQuotations!.Any())
                                             .Where(bc => bc.ValidityId==id && bc.ProductQuotations!
                                                    .All(pq => pq.QuotedValue != 0))
                                             .ExecuteUpdateAsync(y => y
                                                    .SetProperty(bc => bc.StatuId, 8));

        return new ActionResponse<ProductQuotation>
        {
            WasSuccess = true
        };
    }

    public async Task<ActionResponse<bool>> GetAsync(int id, bool lb)
    {
        bool queryable =await _context.ProductQuotations
                                        .AsNoTracking()
                                        .AnyAsync(x => x.BudgetCourse!.ValidityId == id && x.QuotedValue == 0);
          return new ActionResponse<bool>
        {
            WasSuccess = true,
            Result = queryable
        };
    }

}