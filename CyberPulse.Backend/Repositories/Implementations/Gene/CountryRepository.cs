using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Gene;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CountryRepository(ApplicationDbContext context, IWebHostEnvironment env) : base(context)
    {
        _context = context;
        _env = env;
    }

    public async Task<ActionResponse<Country>> GetAsync(int id, bool lb)
    {
        var entity = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Country>()
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Country>()
        {
            WasSuccess = true,
            Result = entity
        };
    }

    public async Task<ActionResponse<Country>> AddAsync(CountryDTO entity)
    {
        var country = new Country
        {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image,
            States = entity.States
        };

        if (!string.IsNullOrWhiteSpace(entity.Image))
        {
            country.Image = await UploadImageAsync(entity.Image, 0);
        }

        _context.Add(country);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = country
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<IEnumerable<Country>> GetComboAsync()
    {
        return await _context.Countries.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Countries.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<Country>> UpdateAsync(CountryDTO entity)
    {
        var country = await _context.Countries.FindAsync(entity.Id);

        if (country == null)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        country.Name = entity.Name;

        if (!string.IsNullOrWhiteSpace(entity.Image))
        {
            country.Image = await UploadImageAsync(entity.Image, entity.Id, country.Image!);
        }

        _context.Update(country);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = country,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Country>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }

    private async Task<string?> UploadImageAsync(string image, int id, string imageOld = "")
    {
        string webRootPath = _env.WebRootPath;

        string directoryFolder = "\\Images\\Countries\\";

        string DirectoryPath = $"{webRootPath}{directoryFolder}";

        var imageBase64 = Convert.FromBase64String(image!);

        string pathImage = $"{Guid.NewGuid()}.jpg";

        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        var path = $"{DirectoryPath}{pathImage}";

        await System.IO.File.WriteAllBytesAsync(path, imageBase64);

        if (id != 0)
        {
            if (!string.IsNullOrWhiteSpace(imageOld))
            {
                System.IO.File.Delete($"{webRootPath}{imageOld}");
            }
        }

        return $"{directoryFolder}{pathImage}";
    }
}
