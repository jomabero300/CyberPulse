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
        await CheckStatusAsync();
        await CheckUnitMeasurementsAsync();
        await CheckBudgetTypesAsync();
        await CheckCountriesAsync();
        await CheckStatesAsync();
        await CheckCitiesAsync();
        await CheckNeighborhoodsAsync();
        await CheckChipAsync();

        await CheckSegmenetsAsync();
        await CheckFamiliesAsync();
        await CheckClassesAsync();

        await CheckProgramsAsync();
        await CheckLotsAsync();
        await CheckProgramLotsAsync();
        await CheckCoursesAsync();
        await CheckCourseProgramLotsAsync();
        await CheckCategoryAsync();
        await CheckProductAsync();
        await CheckRolesAsync();
        
        await CheckUserAsync("Manuel", "Bello", "jbellor@sena.edu.co", "3133670740", UserType.Admi, "17588236");
        //await CheckUserAsync("Manuel", "Bello", "jomabero300@gmail.com", "3133670740", UserType.Admi, "17588236");
        //await CheckUserAsync("Angelina", "Jolie", "angelina@yopmail.com", "3133678526", UserType.Coor, "17588237");
        //await CheckUserAsync("Freddie", "Mercury", "freddie@yopmail.com", "3134568271", UserType.Inst, "17588238");
        //await CheckUserAsync("Felipe", "Pelaes", "felipe@yopmail.com", "3137776666", UserType.Inst, "17588239");
        //await CheckUserAsync("Brad", "Pitt", "brad@yopmail.com", "3129167854", UserType.User, "1029400672");
    }

    private async Task CheckProductAsync()
    {
        if (!_context.Products.Any())
        {
            var sqlScript = File.ReadAllText("Data\\Scripts\\Products.sql");
            await _context.Database.ExecuteSqlRawAsync(sqlScript);
        }
    }

    private async Task CheckCategoryAsync()
    {
        if(!_context.Categories.Any())
        {
            _context.Categories.Add(new Shared.Entities.Inve.Category { Name = "Consumo",Description= "Elementos de uso único, como papelería, productos de limpieza y alimentos esenciales y mas", StatuId=1 });
            _context.Categories.Add(new Shared.Entities.Inve.Category { Name = "Devolutivo", Description= "Activos fijos como escritorios y equipos que no se consumen y se asignan para cumplir funciones.", StatuId=1 });
            _context.Categories.Add(new Shared.Entities.Inve.Category { Name = "Software", Description= "Programas intangibles que constituyen activos de la entidad, distintos de bienes físicos.", StatuId=1 });

            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckCourseProgramLotsAsync()
    {
        if(!_context.CourseProgramLots.Any())
        {
            var sqlScript = File.ReadAllText("Data\\Scripts\\CourseProgramLots.sql");
            await _context.Database.ExecuteSqlRawAsync(sqlScript);
        }
    }

    private async Task CheckCoursesAsync()
    {
        if (!_context.Courses.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Courses.sql");
            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckProgramsAsync()
    {
        if (!_context.InvPrograms.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\InvProgram.sql");
            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckLotsAsync()
    {
        if (!_context.Lots.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Lots.sql");
            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckProgramLotsAsync()
    {
        if (!_context.ProgramLots.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\ProgramLots.sql");
            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckBudgetTypesAsync()
    {
        if (!_context.BudgetTypes.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\BudgetTypes.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckUnitMeasurementsAsync()
    {
        if (!_context.UnitMeasurements.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\UnitMeasurements.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckClassesAsync()
    {
        if (!_context.Classes.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Classes.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckFamiliesAsync()
    {
        if (!_context.Families.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Families.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckSegmenetsAsync()
    {
        if (!_context.Segments.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Segments.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);
        }
    }

    private async Task CheckStatusAsync()
    {
        if (!_context.Status.Any())
        {

            _context.Status.Add(new Statu { Name = "Activo", Nivel = 0 });
            _context.Status.Add(new Statu { Name = "Inactivo", Nivel = 0 });
            _context.Status.Add(new Statu { Name = "Suspendido", Nivel = 0 });
            _context.Status.Add(new Statu { Name = "Cancelado", Nivel = 0 });
            _context.Status.Add(new Statu { Name = "Eliminado", Nivel = 0 });

            _context.Status.Add(new Statu { Name = "Creada", Nivel = 1 });
            _context.Status.Add(new Statu { Name = "Enviada", Nivel = 1 });
            _context.Status.Add(new Statu { Name = "Programando", Nivel = 1 });
            _context.Status.Add(new Statu { Name = "Revisión", Nivel = 1 });
            _context.Status.Add(new Statu { Name = "Rechazada", Nivel = 1 });
            _context.Status.Add(new Statu { Name = "Ejecución", Nivel = 1 });
            _context.Status.Add(new Statu { Name = "Terminada", Nivel = 1 });

            _context.Status.Add(new Statu { Name = "Bajo", Nivel = 2 });
            _context.Status.Add(new Statu { Name = "Alto", Nivel = 2 });

            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOf.CheckRoleAsync(UserType.Admi.ToString());
        await _usersUnitOf.CheckRoleAsync(UserType.Coor.ToString());
        await _usersUnitOf.CheckRoleAsync(UserType.Inst.ToString());
        await _usersUnitOf.CheckRoleAsync(UserType.Purc.ToString());
        await _usersUnitOf.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType, string DocumentId)
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
                Photo= "",
                DocumentId= DocumentId
            };

            await _usersUnitOf.AddUserASync(user, "Mbel123*");

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
            _context.Add(new TypeOfTraining { Name = "Abierta" });
            _context.Add(new TypeOfTraining { Name = "Cerrada" });
            _context.Add(new TypeOfTraining { Name = "Ambas" });

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


        if(!_context.TrainingPrograms.Any())
        {
            _context.TrainingPrograms.Add(new TrainingProgram { Name = "Regular" });
            _context.TrainingPrograms.Add(new TrainingProgram { Name = "Articulación con la media técnica" });
            _context.TrainingPrograms.Add(new TrainingProgram { Name = "Atención A poblacion victima" });
            _context.TrainingPrograms.Add(new TrainingProgram { Name = "Campesena" });
            _context.TrainingPrograms.Add(new TrainingProgram { Name = "Tecno - Academia" });
            _context.TrainingPrograms.Add(new TrainingProgram { Name = "Otro" });
            indEsta = true;
        }

        if (indEsta == true)
        {
            await _context.SaveChangesAsync();
        }
    }

}
