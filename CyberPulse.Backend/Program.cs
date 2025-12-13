using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Implementations;
using CyberPulse.Backend.Repositories.Implementations.Chipp;
using CyberPulse.Backend.Repositories.Implementations.Gene;
using CyberPulse.Backend.Repositories.Implementations.Inve;
using CyberPulse.Backend.Repositories.Interfaces;
using CyberPulse.Backend.Repositories.Interfaces.Chipp;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.Repositories.Interfaces.Inve;
using CyberPulse.Backend.UnitsOfWork.Implementations;
using CyberPulse.Backend.UnitsOfWork.Implementations.Chipp;
using CyberPulse.Backend.UnitsOfWork.Implementations.Gene;
using CyberPulse.Backend.UnitsOfWork.Implementations.Inve;
using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Chipp;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Inve;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            //builder.WithOrigins("https://senarauca.runasp.net")
            //builder.WithOrigins("https://www.senagestionformacion.com", "http://senagestionformacion.com")
            builder.WithOrigins("https://localhost:7244")
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
builder.Services.AddDbContext<ApplicationDbContext>(x =>x.UseSqlServer("name=DefaultConnection"));
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

builder.Services.AddScoped<IIvaRepository, IvaRepository>();
builder.Services.AddScoped<IIvaUnitOfWork, IvaUnitOfWork>();

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



builder.Services.AddScoped<IBudgetCourseRepository, BudgetCourseRepository>();
builder.Services.AddScoped<IBudgetCourseUnitOfWork, BudgetCourseUnitOfWork>();

builder.Services.AddScoped<IBudgetLotRepository, BudgetLotRepository>();
builder.Services.AddScoped<IBudgetLotUnitOfWork, BudgetLotUnitOfWork>();

builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IBudgetUnitOfWork, BudgetUnitOfWork>();

builder.Services.AddScoped<IBudgetProgramRepository, BudgetProgramRepository>();
builder.Services.AddScoped<IBudgetProgramUnitOfWork, BudgetProgramUnitOfWork>();

builder.Services.AddScoped<IBudgetTypeRepository, BudgetTypeRepository>();
builder.Services.AddScoped<IBudgetTypeUnitOfWork, BudgetTypeUnitOfWork>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryUnitOfWork, CategoryUnitOfWork>();

builder.Services.AddScoped<IClasseRepository, ClasseRepository>();
builder.Services.AddScoped<IClasseUnitOfWork, ClasseUnitOfWork>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseUnitOfWork, CourseUnitOfWork>();

builder.Services.AddScoped<ICourseProgramLotRepository, CourseProgramLotRepository>();
builder.Services.AddScoped<ICourseProgramLotUnitOfWork, CourseProgramLotUnitOfWork>();

builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
builder.Services.AddScoped<IFamilyUnitOfWork, FamilyUnitOfWork>();

builder.Services.AddScoped<IInvProgramRepository, InvProgramRepository>();
builder.Services.AddScoped<IInvProgramUnitOfWork, InvProgramUnitOfWork>();

builder.Services.AddScoped<ILotRepository, LotRepository>();
builder.Services.AddScoped<ILotUnitOfWork, LotUnitOfWork>();

builder.Services.AddScoped<IProductCurrentValueRepository, ProductCurrentValueRepository>();
builder.Services.AddScoped<IProductCurrentValueUnitOfWork, ProductCurrentValueUnitOfWork>();

builder.Services.AddScoped<IProductQuotationBodyRepository, ProductQuotationBodyRepository>();
builder.Services.AddScoped<IProductQuotationBodyUnitOfWork, ProductQuotationBodyUnitOfWork>();

builder.Services.AddScoped<IProductQuotationRepository, ProductQuotationRepository>();
builder.Services.AddScoped<IProductQuotationUnitOfWork, ProductQuotationUnitOfWork>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();

builder.Services.AddScoped<IProgramLotRepository, ProgramLotRepository>();
builder.Services.AddScoped<IProgramLotUnitOfWork, ProgramLotUnitOfWork>();

builder.Services.AddScoped<ISegmentRepository, SegmentRepository>();
builder.Services.AddScoped<ISegmentUnitOfWork, SegmentUnitOfWork>();

builder.Services.AddScoped<IUnitMeasurementRepository, UnitMeasurementRepository>();
builder.Services.AddScoped<IUnitMeasurementUnitOfWork, UnitMeasurementUnitOfWork>();

builder.Services.AddScoped<IValidityRepository, ValidityRepository>();
builder.Services.AddScoped<IValidityUnitOfWork, ValidityUnitOfWork>();



builder.Services.AddScoped<IMailHelper, MailHelper>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    x.SignIn.RequireConfirmedEmail = true;
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = true;
    x.Password.RequireNonAlphanumeric = true;
    x.Password.RequireUppercase = true;
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    x.Lockout.MaxFailedAccessAttempts = 3;
    x.Lockout.AllowedForNewUsers = true;
    x.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserValidator<CustomEmailValidator>();

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

builder.Services.AddLocalization(option => option.ResourcesPath= "Resources");

var app = builder.Build();

app.UseRouting();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseFileServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
