using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class SegmentUnitOfWork : GenericUnitOfWork<Segment>, ISegmentUnitOfWork
{
    private readonly ISegmentRepository _segmentRepository;
    public SegmentUnitOfWork(IGenericRepository<Segment> repository, ISegmentRepository segmentRepository) : base(repository)
    {
        _segmentRepository = segmentRepository;
    }


    public override async Task<ActionResponse<Segment>> GetAsync(int id)=>await _segmentRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Segment>>> GetAsync(PaginationDTO pagination)=>await _segmentRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Segment>> DeleteAsync(int id)=>await _segmentRepository.DeleteAsync(id);

    public async Task<ActionResponse<Segment>> AddAsync(SegmentDTO entity)=>await _segmentRepository.AddAsync(entity);
    public async Task<IEnumerable<Segment>> GetComboAsync()=>await _segmentRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _segmentRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<Segment>> UpdateAsync(SegmentDTO entity)=>await _segmentRepository.UpdateAsync(entity);
}
