using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;

public class TypeOfTrainingUnitOfWork : GenericUnitOfWork<TypeOfTraining>, ITypeOfTrainingUnitOfWork
{
    private readonly ITypeOfTrainingRepository _typeOfTraining;

    public TypeOfTrainingUnitOfWork(IGenericRepository<TypeOfTraining> repository, ITypeOfTrainingRepository typeOfTraining) : base(repository)
    {
        _typeOfTraining = typeOfTraining;
    }

    public async Task<IEnumerable<TypeOfTraining>> GetComboAsync() => await _typeOfTraining.GetComboAsync();
}
