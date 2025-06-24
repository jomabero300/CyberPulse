using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Chipp;
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
        await CheckChipAsync();

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
    private async Task CheckChipAsync()
    {
        bool indEsta = false;
        if (!_context.PriorityBets.Any())
        {
            _context.Add(new PriorityBet { Name = "Apuesta del sector" });
            _context.Add(new PriorityBet { Name = "Economía popular" });
            _context.Add(new PriorityBet { Name = "Fortalecimiento en programas TIC" });
            _context.Add(new PriorityBet { Name = "No Aplica" });
            _context.Add(new PriorityBet { Name = "Transición energética" });
            indEsta = true;
        }

        if (!_context.TriningLevels.Any())
        {
            _context.Add(new TriningLevel { Name = "AUXILIAR" });
            _context.Add(new TriningLevel { Name = "COMPLEMENTARIA VIRTUAL" });
            _context.Add(new TriningLevel { Name = "CURSO ESPECIAL" });
            _context.Add(new TriningLevel { Name = "ESPECIALIZACIÓN TECNOLÓGICA" });
            _context.Add(new TriningLevel { Name = "OPERARIO" });
            _context.Add(new TriningLevel { Name = "PROFUNDIZACIÓN TÉCNICA" });
            _context.Add(new TriningLevel { Name = "TÉCNICO" });
            _context.Add(new TriningLevel { Name = "TECNÓLOGO" });

            indEsta = true;
        }

        if (!_context.TypeOfTraining.Any())
        {
            _context.Add(new TypeOfTraining { Name = "Aula movil" });
            _context.Add(new TypeOfTraining { Name = "Campesena" });
            _context.Add(new TypeOfTraining { Name = "Evento de Divulgación tecnológica" });
            _context.Add(new TypeOfTraining { Name = "Formacion complementaria regulad" });
            _context.Add(new TypeOfTraining { Name = "Otro" });
            _context.Add(new TypeOfTraining { Name = "Tecno - Academia" });
            _context.Add(new TypeOfTraining { Name = "Victimas" });

            indEsta = true;
        }

        if (!_context.TypeOfPoblations.Any())
        {
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Adolescente en conflicto con la ley penal" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Adolescente trabajador" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Adolescentes desvinculados de grupos armados organizados" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Afrocolombianos desplazados por la violencia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Afrocolombianos desplazados por la violencia cabeza de familia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Artesanos" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Desplazados discapacitados" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Desplazados por fenómenos naturales" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Desplazados por fenómenos naturales cabeza de familia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Desplazados por la violencia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Desplazados por la violencia cabeza de familia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Discapacitado cognitivo" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Discapacitado limitación auditiva o sorda" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Discapacitado limitación física o motora" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Discapacitado limitación visual o ciega" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Discapacitados" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Discapacitados mental" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Emprendedores" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Indígenas" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Indígenas desplazados por la violencia " });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Indígenas desplazados por la violencia cabeza de familia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "INPEC" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Jóvenes vulnerables" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Microempresas" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Mujer cabeza de familia" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Negritudes" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Ninguna " });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Personas en proceso de reintegración" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Remitidos por el PAL" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Remitidos por el Servicio Nacional de Empleo" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Sobrevivientes minas antipersonales" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Soldados campesinos" });
            _context.TypeOfPoblations.Add(new TypeOfPoblation { Name = "Tercera edad" });
            indEsta = true;
        }

        if (indEsta == true)
        {
            await _context.SaveChangesAsync();
        }
    }

}
