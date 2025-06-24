using CyberPulse.Shared.Entities.Chipp;

namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;

public interface ITypeOfTrainingUnitOfWork
{
    Task<IEnumerable<TypeOfTraining>> GetComboAsync();
}
