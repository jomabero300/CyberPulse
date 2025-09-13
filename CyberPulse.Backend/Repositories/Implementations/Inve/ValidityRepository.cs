using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Responses;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Inve;

public class ValidityRepository : GenericRepository<Validity>, IValidityRepository
{
    private readonly ApplicationDbContext _context;
    public ValidityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Validity>> GetAsync(int id)
    {
        var entity = await _context.Validities
            .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Validity>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<IEnumerable<Validity>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Validities.AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Value ==int.Parse(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Validity>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Value)
                .Paginate(pagination)
                .ToListAsync()
        };

    }
    public override async Task<ActionResponse<Validity>> DeleteAsync(int id)
    {
        var entity = await _context.Validities.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Validity>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<Validity>> AddAsync(ValidityDTO entity)
    {
        var model = new Validity
        {
            Id = entity.Id,
            Value=entity.Value,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Validity>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<IEnumerable<Validity>> GetComboAsync()
    {
        return await _context.Validities.AsNoTracking()
                                        .OrderBy(x => x.Value)
                                        .ToListAsync();
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Validities.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Value==int.Parse(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public async Task<ActionResponse<Validity>> UpdateAsync(ValidityDTO entity)
    {
        var model = await _context.Validities.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        //preguntar si es para cerrar la vigencia


        if(entity.StatuId==6)
        {
            var movimieto=await _context.Budgets.FirstOrDefaultAsync(x=>x.ValidityId==entity.Id);
            if (movimieto == null)
            {
                return new ActionResponse<Validity>
                {
                    WasSuccess = false,
                    Message = "ERR013",
                };
            }
            //buscr si tiene regitro de la vigencia
        }



        model.Value = entity.Value;
        model.StatuId = entity.StatuId;


        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Validity>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Validity>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ActionResponse<int>> GetNewValidityAsync()
    {

        //Validar si hay registro
        var vigenciaUltima=await _context.Validities.AsNoTracking().OrderByDescending(x => x.Value).FirstOrDefaultAsync();

        if(vigenciaUltima == null)
        {
            return new ActionResponse<int>
            {
                WasSuccess = false,
                Message="ERRO11"
            };
        }

        var vigenciaActivA=await _context.Validities.AsNoTracking().FirstOrDefaultAsync(x=>x.StatuId==1);

        if (vigenciaActivA != null)
        {
            return new ActionResponse<int>
            {
                WasSuccess = false,
                Message = "ERRO12"
            };
        }

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)(vigenciaUltima.Value+1)
        };
    }
}
