using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Inve;

public interface IUnitMeasurementRepository
{
    Task<ActionResponse<UnitMeasurement>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<UnitMeasurement>>> GetAsync();

    Task<ActionResponse<UnitMeasurement>> AddAsync(UnitMeasurementDTO entity);

    Task<ActionResponse<UnitMeasurement>> UpdateAsync(UnitMeasurementDTO entity);

    Task<ActionResponse<UnitMeasurement>> DeleteAsync(int id);

    Task<IEnumerable<UnitMeasurement>> GetComboAsync();

    Task<ActionResponse<IEnumerable<UnitMeasurement>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
