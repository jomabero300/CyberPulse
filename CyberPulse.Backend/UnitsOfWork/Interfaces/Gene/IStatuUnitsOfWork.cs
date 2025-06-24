using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.GeneDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface IStatuUnitsOfWork
{
    Task<ActionResponse<Statu>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<Statu>>> GetAsync();


    //Task<ActionResponse<Statu>> AddAsync(StatuDTO statuDTO);

    //Task<ActionResponse<Statu>> UpdateAsync(StatuDTO statuDTO);

    Task<IEnumerable<Statu>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Statu>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

}
