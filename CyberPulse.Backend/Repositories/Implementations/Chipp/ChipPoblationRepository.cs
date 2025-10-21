using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class ChipPoblationRepository : GenericRepository<ChipPoblation>, IChipPoblationRepository
{
    private readonly ApplicationDbContext _context;
    public ChipPoblationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ActionResponse<ChipPoblation>> AddAsync(ChipPoblationDTO entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ChipPoblation>> GetComboAsync()
    {
        throw new NotImplementedException();

    }

    public Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        throw new NotImplementedException();
    }
}
