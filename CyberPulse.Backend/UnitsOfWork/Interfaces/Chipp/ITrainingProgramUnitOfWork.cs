using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;

public interface ITrainingProgramUnitOfWork
{
    Task<ActionResponse<TrainingProgram>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync();
    Task<ActionResponse<TrainingProgram>> DeleteAsync(int id);
    Task<IEnumerable<TrainingProgram>> GetComboAsync();
    Task<ActionResponse<IEnumerable<TrainingProgram>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
