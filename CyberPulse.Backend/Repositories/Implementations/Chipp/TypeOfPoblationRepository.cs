using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Chipp;
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
            Result = await _context.TypeOfPoblations.AsNoTracking().OrderBy(x => x.Name).ToListAsync(),
        };
    }

    public async Task<IEnumerable<TypeOfPoblationDTO>> GetAsync(string filter)
    {
        var response = await _context.TypeOfPoblations.AsNoTracking().Where(x=>x.Name.ToLower()!= filter).ToListAsync();
        

        var typeOfPoblationDto = response.Select(x => new TypeOfPoblationDTO
        {
            Id = x.Id,
            Name = x.Name,
            Quantity = 0,
        }).ToList();

        return typeOfPoblationDto;
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
