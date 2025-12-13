using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class CourseProgramLotUnitOfWork : GenericUnitOfWork<CourseProgramLot>, ICourseProgramLotUnitOfWork
{
    private readonly ICourseProgramLotRepository _courseLotRepository;
    public CourseProgramLotUnitOfWork(IGenericRepository<CourseProgramLot> repository, ICourseProgramLotRepository courseLotRepository) : base(repository)
    {
        _courseLotRepository = courseLotRepository;
    }

    public override async Task<ActionResponse<CourseProgramLot>> GetAsync(int id)=>await _courseLotRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<CourseProgramLot>>> GetAsync(PaginationDTO pagination)=>await _courseLotRepository.GetAsync(pagination);
    public override async Task<ActionResponse<CourseProgramLot>> DeleteAsync(int id)=>await _courseLotRepository.DeleteAsync(id);

    public async Task<ActionResponse<CourseProgramLot>> AddAsync(CourseProgramLotDTO entity)=>await _courseLotRepository.AddAsync(entity);
    public async Task<IEnumerable<CourseProgramLot>> GetComboAsync(int id)=>await _courseLotRepository.GetComboAsync(id);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _courseLotRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<IEnumerable<CourseProgramLot>>> GetAsync(string Filter) => await _courseLotRepository.GetAsync(Filter);
    public async Task<ActionResponse<CourseProgramLot>> UpdateAsync(CourseProgramLotDTO entity)=>await _courseLotRepository.UpdateAsync(entity);

}
