using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Inve;

public class LotUnitOfWork : GenericUnitOfWork<Lot>, ILotUnitOfWork
{
    private readonly ILotRepository _lotRepository;
    public LotUnitOfWork(IGenericRepository<Lot> repository, ILotRepository lotRepository) : base(repository)
    {
        _lotRepository = lotRepository;
    }

    public override async Task<ActionResponse<Lot>> GetAsync(int id)=>await _lotRepository.GetAsync(id);
    public override async Task<ActionResponse<IEnumerable<Lot>>> GetAsync(PaginationDTO pagination)=>await _lotRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Lot>> DeleteAsync(int id)=>await _lotRepository.DeleteAsync(id);


    public async Task<ActionResponse<Lot>> AddAsync(LotDTO entity)=>await _lotRepository.AddAsync(entity);
    public async Task<IEnumerable<Lot>> GetComboAsync() => await _lotRepository.GetComboAsync();
    //public async Task<IEnumerable<Lot2DTO>> GetComboCourseAsync(int id)=>await _lotRepository.GetComboCourseAsync(id);
    public async Task<IEnumerable<Lot>> GetComboAsync(int id,bool indEsta)=> await _lotRepository.GetComboAsync(id,indEsta);
    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _lotRepository.GetTotalRecordsAsync(pagination);
    public async Task<ActionResponse<Lot>> UpdateAsync(LotDTO entity)=>await _lotRepository.UpdateAsync(entity);
}