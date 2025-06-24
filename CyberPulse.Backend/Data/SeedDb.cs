using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Data;

public class SeedDb
{
    private readonly ApplicationDbContext _context;
    private readonly IUsersUnitOfWork _usersUnitOf;

    public SeedDb(ApplicationDbContext context, IUsersUnitOfWork usersUnitOf)
    {
        _context = context;
        _usersUnitOf = usersUnitOf;
    }
    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckStatesAsync();
        await CheckCitiesAsync();
        await CheckNeighborhoodsAsync();

        //await CheckStatesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("Marcos", "Suarez", "marcos301234@gmail.com", "3133670740", UserType.Admin);
    }


    private async Task CheckRolesAsync()
    {
        await _usersUnitOf.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOf.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
    {
        var user = await _usersUnitOf.GetUserAsync(email);

        if (user == null)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Colombia");

            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Country = country!,
                UserType = userType,
                Photo= ""
            };

            await _usersUnitOf.AddUserASync(user, "12345678");

            await _usersUnitOf.AddUserToRoleAsync(user, userType.ToString());

            var token = await _usersUnitOf.GenerateEmailConfirmationTokenAsync(user);
            await _usersUnitOf.ConfirmEmailAsync(user, token);
        }

        return user;
    }


    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Countries.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }

    private async Task CheckStatesAsync()
    {
        if (!_context.States.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\States.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }
    private async Task CheckCitiesAsync()
    {
        if (!_context.Cities.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Cities.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }
    private async Task CheckNeighborhoodsAsync()
    {
        if (!_context.Neighborhoods.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Neighborhoods.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }

}
