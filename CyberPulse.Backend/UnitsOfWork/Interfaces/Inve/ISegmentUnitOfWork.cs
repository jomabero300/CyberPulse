using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;

public interface ISegmentUnitOfWork
{
    Task<ActionResponse<Segment>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Segment>>> GetAsync();

    Task<ActionResponse<Segment>> AddAsync(SegmentDTO entity);

    Task<ActionResponse<Segment>> UpdateAsync(SegmentDTO entity);

    Task<ActionResponse<Segment>> DeleteAsync(int id);

    Task<IEnumerable<Segment>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Segment>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}