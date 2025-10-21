using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;

public class TypeOfPoblationUnitOfWork : GenericUnitOfWork<TypeOfPoblation>, ITypeOfPoblationUnitOfWork
{
    private readonly ITypeOfPoblationRepository _typeOfPoblation;
    public TypeOfPoblationUnitOfWork(IGenericRepository<TypeOfPoblation> repository, ITypeOfPoblationRepository typeOfPoblation) : base(repository)
    {
        _typeOfPoblation = typeOfPoblation;
    }

    public override async Task<ActionResponse<IEnumerable<TypeOfPoblation>>> GetAsync()=>await _typeOfPoblation.GetAsync();

    public async Task<IEnumerable<TypeOfPoblationDTO>> GetAsync(string url) => await _typeOfPoblation.GetAsync(url);

    public Task<IEnumerable<TypeOfPoblation>> GetComboAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        throw new NotImplementedException();
    }
}
