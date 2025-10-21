using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Gene;

public class CityUnitOfWork : GenericUnitOfWork<City>, ICityUnitOfWork
{
    private readonly ICityRepository _cityRepository;
    public CityUnitOfWork(IGenericRepository<City> repository, ICityRepository cityRepository) : base(repository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<City>> GetComboAsync(int id) => await _cityRepository.GetComboAsync(id);

}
