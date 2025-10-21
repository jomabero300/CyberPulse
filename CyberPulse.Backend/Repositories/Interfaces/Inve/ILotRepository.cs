using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface ILotRepository
{
    Task<ActionResponse<Lot>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Lot>>> GetAsync();

    Task<ActionResponse<Lot>> AddAsync(LotDTO entity);

    Task<ActionResponse<Lot>> UpdateAsync(LotDTO entity);

    Task<ActionResponse<Lot>> DeleteAsync(int id);

    //Task<IEnumerable<Lot>> GetComboAsync();
    //Task<IEnumerable<Lot2DTO>> GetComboCourseAsync(int id);
    Task<IEnumerable<Lot>> GetComboAsync(int id,bool indEsta);

    Task<ActionResponse<IEnumerable<Lot>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
