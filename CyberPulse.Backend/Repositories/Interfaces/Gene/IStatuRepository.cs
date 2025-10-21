using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Gene;

public interface IStatuRepository
{
    Task<ActionResponse<Statu>> GetAsync(int id);
    Task<ActionResponse<Statu>> GetAsync(string name,int nivel);
    Task<ActionResponse<IEnumerable<Statu>>> GetAsync();

    Task<ActionResponse<Statu>> AddAsync(Statu entity);

    Task<ActionResponse<Statu>> DeleteAsync(int id);

    Task<IEnumerable<Statu>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<Statu>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
