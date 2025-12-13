using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class CourseUnitOfWork : GenericUnitOfWork<Course>, ICourseUnitOfWork
{
    private readonly ICourseRepository _courseRepository;
    public CourseUnitOfWork(IGenericRepository<Course> repository, ICourseRepository courseRepository) : base(repository)
    {
        _courseRepository = courseRepository;
    }

    public override async Task<ActionResponse<Course>> GetAsync(int id)=>await _courseRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Course>>> GetAsync(PaginationDTO pagination)=>await _courseRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Course>> DeleteAsync(int id)=>await _courseRepository.DeleteAsync(id);

    public async Task<ActionResponse<Course>> AddAsync(CourseDTO entity)=>await _courseRepository.AddAsync(entity);
    public async Task<ActionResponse<IEnumerable<Course>>> GetAsync(string Filter) => await _courseRepository.GetAsync(Filter);
    public async Task<IEnumerable<Course>> GetComboAsync(int id,bool indEsta)=>await _courseRepository.GetComboAsync(id,indEsta);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _courseRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<Course>> UpdateAsync(CourseDTO entity)=>await _courseRepository.UpdateAsync(entity);

}
