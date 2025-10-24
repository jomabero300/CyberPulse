using CyberPulse.Shared.EntitiesDTO.Inve;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface IProductQuotationBodyUnitOfWork
{
    Task<IEnumerable<ProductQuotationBodyDTO>> GetComboAsync(int id);
}