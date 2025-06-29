using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Chipp;

public class TypeOfPoblationRepository : GenericRepository<TypeOfPoblation>, ITypeOfPoblationRepository
{
    private readonly ApplicationDbContext _context;
    public TypeOfPoblationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public override async Task<ActionResponse<IEnumerable<TypeOfPoblation>>> GetAsync()
    {
        return new ActionResponse<IEnumerable<TypeOfPoblation>>
        {
            WasSuccess = true,
            Result = await _context.TypeOfPoblations.OrderBy(x => x.Name).ToListAsync(),
        };
    }
    public Task<IEnumerable<TypeOfPoblation>> GetComboAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        throw new NotImplementedException();
    }
}
