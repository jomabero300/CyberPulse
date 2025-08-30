using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IClasseRepository
{
    Task<ActionResponse<Classe>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Classe>>> GetAsync();

    Task<ActionResponse<Classe>> AddAsync(ClasseDTO entity);

    Task<ActionResponse<Classe>> UpdateAsync(ClasseDTO entity);

    Task<ActionResponse<Classe>> DeleteAsync(int id);

    Task<IEnumerable<Classe>> GetComboAsync(int id);

    Task<ActionResponse<IEnumerable<Classe>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}