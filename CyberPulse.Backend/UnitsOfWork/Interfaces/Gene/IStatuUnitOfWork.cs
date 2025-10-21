using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface IStatuUnitOfWork
{
    Task<ActionResponse<Statu>> GetAsync(int id);
    Task<ActionResponse<Statu>> GetAsync(string name, int nivel);
    Task<ActionResponse<IEnumerable<Statu>>> GetAsync();

    Task<ActionResponse<Statu>> DeleteAsync(int id);

    Task<IEnumerable<Statu>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<Statu>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
