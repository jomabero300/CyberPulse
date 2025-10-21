using CyberPulse.Shared.Entities.Chipp;

namespace CyberPulse.Backend.Repositories.Interfaces.Chipp;

public interface ITypeOfTrainingRepository
{
    Task<IEnumerable<TypeOfTraining>> GetComboAsync();

}
