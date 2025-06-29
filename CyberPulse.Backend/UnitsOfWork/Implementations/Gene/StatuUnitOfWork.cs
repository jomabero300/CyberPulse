﻿using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Gene;

public class StatuUnitOfWork : GenericUnitOfWork<Statu>, IStatuUnitOfWork
{
    private readonly IStatuRepository _statuRepository;

    public StatuUnitOfWork(IGenericRepository<Statu> repository, IStatuRepository statuRepository) : base(repository)
    {
        _statuRepository = statuRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Statu>>> GetAsync(PaginationDTO pagination) => await _statuRepository.GetAsync(pagination);

    public async Task<IEnumerable<Statu>> GetComboAsync() => await _statuRepository.GetComboAsync();

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _statuRepository.GetTotalRecordsAsync(pagination);
}
