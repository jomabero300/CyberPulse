using CyberPulse.Shared.EntitiesDTO.Inve;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IProductQuotationBodyRepository
{
    Task<IEnumerable<ProductQuotationBodyDTO>> GetComboAsync(int id);
}