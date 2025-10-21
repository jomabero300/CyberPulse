using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class UnitMeasurementUnitOfWork : GenericUnitOfWork<UnitMeasurement>, IUnitMeasurementUnitOfWork
{
    private readonly IUnitMeasurementRepository _unitMeasurementRepository;
    public UnitMeasurementUnitOfWork(IGenericRepository<UnitMeasurement> repository, IUnitMeasurementRepository unitMeasurementRepository) : base(repository)
    {
        _unitMeasurementRepository = unitMeasurementRepository;
    }
    public override async Task<ActionResponse<UnitMeasurement>> GetAsync(int id)=>await _unitMeasurementRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<UnitMeasurement>>> GetAsync(PaginationDTO pagination) => await _unitMeasurementRepository.GetAsync(pagination);
    public override async Task<ActionResponse<UnitMeasurement>> DeleteAsync(int id)=>await _unitMeasurementRepository.DeleteAsync(id);


    public async Task<ActionResponse<UnitMeasurement>> AddAsync(UnitMeasurementDTO entity)=>await _unitMeasurementRepository.AddAsync(entity);
    public async Task<IEnumerable<UnitMeasurement>> GetComboAsync()=>await _unitMeasurementRepository.GetComboAsync();
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _unitMeasurementRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<UnitMeasurement>> UpdateAsync(UnitMeasurementDTO entity)=>await _unitMeasurementRepository.UpdateAsync(entity);
}
