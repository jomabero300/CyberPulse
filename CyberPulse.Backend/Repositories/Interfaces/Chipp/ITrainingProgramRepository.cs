using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface ITrainingProgramRepository
{
    Task<ActionResponse<TrainingProgram>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync();

    Task<ActionResponse<TrainingProgram>> AddAsync(TrainingProgramDTO entity);

    Task<ActionResponse<TrainingProgram>> UpdateAsync(TrainingProgramDTO entity);

    Task<ActionResponse<TrainingProgram>> DeleteAsync(int id);
    Task<IEnumerable<TrainingProgram>> GetComboAsync();
    Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
