using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class CourseLotUnitOfWork : GenericUnitOfWork<CourseLot>, ICourseLotUnitOfWork
{
    private readonly ICourseLotRepository _courseLotRepository;
    public CourseLotUnitOfWork(IGenericRepository<CourseLot> repository, ICourseLotRepository courseLotRepository) : base(repository)
    {
        _courseLotRepository = courseLotRepository;
    }

    public override async Task<ActionResponse<CourseLot>> GetAsync(int id)=>await _courseLotRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<CourseLot>>> GetAsync(PaginationDTO pagination)=>await _courseLotRepository.GetAsync(pagination);
    public override async Task<ActionResponse<CourseLot>> DeleteAsync(int id)=>await _courseLotRepository.DeleteAsync(id);

    public async Task<ActionResponse<CourseLot>> AddAsync(CourseLotDTO entity)=>await _courseLotRepository.AddAsync(entity);
    public async Task<IEnumerable<CourseLot>> GetComboAsync(int id)=>await _courseLotRepository.GetComboAsync(id);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _courseLotRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<CourseLot>> UpdateAsync(CourseLotDTO entity)=>await _courseLotRepository.UpdateAsync(entity);
}
