﻿using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;

public class ChipProgramUnitOfWork : GenericUnitOfWork<ChipProgram>, IChipProgramUnitOfWork
{
    private readonly IChipProgramRepository _chipProgramRepository;

    public ChipProgramUnitOfWork(IGenericRepository<ChipProgram> repository, IChipProgramRepository chipProgramRepository) : base(repository)
    {
        _chipProgramRepository = chipProgramRepository;
    }

    public async Task<ActionResponse<ChipProgram>> GetAsync(string code)=>await _chipProgramRepository.GetAsync(code);

    public async Task<IEnumerable<ChipProgram>> GetComboAsync(int id) => await _chipProgramRepository.GetComboAsync(id);
}
