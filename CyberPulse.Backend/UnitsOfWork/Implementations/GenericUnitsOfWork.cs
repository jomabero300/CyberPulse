using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Shared.EntitiesDTO.GeneDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations;

public class GenericUnitsOfWork<T> : IGenericUnitsOfWork<T> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericUnitsOfWork(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<ActionResponse<T>> AddAsync(T entity)=>await _repository.AddAsync(entity);

    public async Task<ActionResponse<T>> DeleteAsync(int id)=>await _repository.DeleteAsync(id);

    public async Task<ActionResponse<T>> GetAsync(int id)=>await _repository.GetAsync(id);

    public async Task<ActionResponse<IEnumerable<T>>> GetAsync()=>await _repository.GetAsync();

    public async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)=>await _repository.GetAsync(pagination);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync()=>await _repository.GetTotalRecordsAsync();

    public async Task<ActionResponse<T>> UpdateAsync(T entity)=>await _repository.UpdateAsync(entity);
}
