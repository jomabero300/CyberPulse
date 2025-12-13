using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ProductQuotationBodyRepository : GenericRepository<ProductQuotationBodyDTO>, IProductQuotationBodyRepository
{
    private readonly ApplicationDbContext _context;
    public ProductQuotationBodyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductQuotationBodyDTO>> GetComboAsync(int id)
    {
        var lots = await _context.BudgetCourses
                        .AsNoTracking()
                        .Include(x => x.CourseProgramLot).ThenInclude(x => x!.ProgramLot)
                        .Where(x => x.Id == id)
                        .Select(x => x.CourseProgramLot!.ProgramLot!.LotId)
                        .ToListAsync();

        var products = await _context.ProductCurrentValues
                        .AsNoTracking()
                        .Include(x => x.Product).ThenInclude(x => x!.UnitMeasurement)
                        .Where(x => lots.Contains(x.Product!.LotId))
                        .Select(x => new ProductQuotationBodyDTO
                        {
                            Id = 0,
                            BudgetCourseId = id,
                            ProductCurrentValueId = x.Id,
                            RequestedQuantity = 0,
                            AcceptedQuantity = 0,
                            Work = x.Worth,
                            ProductId = x.ProductId,
                            ProductName = x.Product!.Name,
                            ProductDescription = x.Product!.Description,
                            UnitMeasurementName = x.Product!.UnitMeasurement!.Name,
                            Quoted01=0,
                            Quoted02=0,
                            Quoted03=0,
                            StatuId=1
                        })
                        .ToListAsync();

        var productQuotatoin = await _context.ProductQuotations
                        .AsNoTracking()
                        .Where(x => x.BudgetCourseId == id)
                        .ToListAsync();

        if (productQuotatoin.Count > 0)
        {
            foreach (var quotation in productQuotatoin)
            {
                var product = products.FirstOrDefault(x => x.ProductCurrentValueId == quotation.ProductCurrentValueId);
                if (product != null)
                {
                    product.Id = quotation.Id;
                    product.RequestedQuantity = quotation.AcceptedQuantity==0? quotation.RequestedQuantity: quotation.AcceptedQuantity;
                    product.AcceptedQuantity = quotation.AcceptedQuantity;
                    product.QuotedValue = quotation.QuotedValue;
                    product.Quoted01 = quotation.Quoted01;
                    product.Quoted02 = quotation.Quoted02;
                    product.Quoted03 = quotation.Quoted03;
                    product.StatuId= quotation.StatuId;
                    product.Work = quotation.QuotedValue;
                }
            }
        }

        return products;
    }
}
