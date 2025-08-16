using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;

public class TrainingProgramUnitOfWork : GenericUnitOfWork<TrainingProgram>, ITrainingProgramUnitOfWork
{
    private readonly ITrainingProgramRepository _trainingProgramRepository;
    public TrainingProgramUnitOfWork(IGenericRepository<TrainingProgram> repository, ITrainingProgramRepository trainingProgramRepository) : base(repository)
    {
        _trainingProgramRepository = trainingProgramRepository;
    }

    public async Task<ActionResponse<TrainingProgram>> AddAsync(TrainingProgramDTO entity) => await _trainingProgramRepository.AddAsync(entity);

    public override async Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync(PaginationDTO pagination) => await _trainingProgramRepository.GetAsync(pagination);

    public async Task<IEnumerable<TrainingProgram>> GetComboAsync()=>await _trainingProgramRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _trainingProgramRepository.GetTotalRecordsAsync(pagination);

    public async Task<ActionResponse<TrainingProgram>> UpdateAsync(TrainingProgramDTO entity) => await _trainingProgramRepository.UpdateAsync(entity);
}
