using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Implementations;
using CyberPulse.Backend.Repositories.Implementations.Chipp;
using CyberPulse.Backend.Repositories.Implementations.Gene;
using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Implementations;
using CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;
using CyberPulse.Backend.UnitsOfWork.Implementations.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            //builder.WithOrigins("https://senarauca.runasp.net")
            //builder.WithOrigins("https://localhost:7244")
            builder.WithOrigins("https://www.senagestionformacion.com", "http://senagestionformacion.com")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
            // .AllowCredentials(); // Si necesitas enviar cookies o cabeceras de autorización, descomenta esta línea.
        });

    // Si realmente quieres permitir cualquier origen (menos seguro para producción):
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Permite cualquier origen
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer("name=DefaultConnection"));
builder.Services.AddTransient<SeedDb>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));

builder.Services.AddScoped<IMailHelper, MailHelper>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityUnitOfWork, CityUnitOfWork>();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryUnitOfWork, CountryUnitOfWork>();

builder.Services.AddScoped<IExcelExportRepository, ExcelExportRepository>();
builder.Services.AddScoped<IExcelExportUnitOfWork, ExcelExportUnitOfWork>();

builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
builder.Services.AddScoped<INeighborhoodUnitOfWork, NeighborhoodUnitOfWork>();

builder.Services.AddScoped<IStatuRepository, StatuRepository>();
builder.Services.AddScoped<IStatuUnitOfWork, StatuUnitOfWork>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();



builder.Services.AddScoped<ITypeOfPoblationRepository, TypeOfPoblationRepository>();
builder.Services.AddScoped<ITypeOfPoblationUnitOfWork, TypeOfPoblationUnitOfWork>();

builder.Services.AddScoped<IChipProgramRepository, ChipProgramRepository>();
builder.Services.AddScoped<IChipProgramUnitOfWork, ChipProgramUnitOfWork>();

builder.Services.AddScoped<IChipRepository, ChipRepository>();
builder.Services.AddScoped<IChipUnitOfWork, ChipUnitOfWork>();

builder.Services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();
builder.Services.AddScoped<ITrainingProgramUnitOfWork, TrainingProgramUnitOfWork>();

builder.Services.AddScoped<ITypeOfTrainingRepository, TypeOfTrainingRepository>();
builder.Services.AddScoped<ITypeOfTrainingUnitOfWork, TypeOfTrainingUnitOfWork>();



builder.Services.AddScoped<IMailHelper, MailHelper>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    x.SignIn.RequireConfirmedEmail = true;
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    x.Lockout.MaxFailedAccessAttempts = 3;
    x.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
    //.AddUserValidator<CustomEmailValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwMBELtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");


SeedData(app);
void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
    var service = scope.ServiceProvider.GetService<SeedDb>();
    service!.SeedAsync().Wait();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


//app.UseCors(x => x
//    .AllowAnyHeader()
//    .AllowAnyMethod()
//    .SetIsOriginAllowed(origin => true)
//    .AllowCredentials());


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseFileServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
