using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface IIvaUnitOfWork
{
    Task<ActionResponse<Iva>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Iva>>> GetAsync();

    Task<ActionResponse<Iva>> AddAsync(IvaDTO entity);

    Task<ActionResponse<Iva>> UpdateAsync(IvaDTO entity);

    Task<ActionResponse<Iva>> DeleteAsync(int id);

    Task<IEnumerable<Iva>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Iva>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}