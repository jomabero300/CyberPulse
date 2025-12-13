using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Entities.Inve;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Chip> Chips { get; set; }
    public DbSet<ChipPoblation> ChipPoblations { get; set; }
    public DbSet<ChipProgram> ChipPrograms { get; set; }
    public DbSet<PriorityBet> PriorityBets { get; set; }
    public DbSet<TrainingProgram> TrainingPrograms { get; set; }
    public DbSet<TriningLevel> TriningLevels { get; set; }
    public DbSet<TypeOfPoblation> TypeOfPoblations { get; set; }
    public DbSet<TypeOfTraining> TypeOfTraining { get; set; }


    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Iva> Ivas { get; set; }
    public DbSet<Neighborhood> Neighborhoods { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Statu> Status { get; set; }



    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetCourse> BudgetCourses { get; set; }
    public DbSet<BudgetLot> BudgetLots { get; set; }
    public DbSet<BudgetProgram> BudgetPrograms { get; set; }
    public DbSet<BudgetType> BudgetTypes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Classe> Classes { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseProgramLot> CourseProgramLots { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<InvProgram> InvPrograms { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<Product> Products{ get; set; }
    public DbSet<ProductCurrentValue> ProductCurrentValues { get; set; }
    public DbSet<ProductQuotation> ProductQuotations { get; set; }
    public DbSet<ProgramLot> ProgramLots { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<UnitMeasurement> UnitMeasurements { get; set; }
    public DbSet<Validity>  Validities { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Admi");

        builder.Entity<ChipPoblation>().HasIndex(x => new { x.TypePoblationId, x.ChipId }).IsUnique();
        builder.Entity<ChipProgram>().HasIndex(x => new {x.Code,x.Version }).IsUnique();
        builder.Entity<PriorityBet>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<TrainingProgram>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<TriningLevel>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<TypeOfPoblation>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<TypeOfTraining>().HasIndex(x => x.Name).IsUnique();

        //builder.Entity<Calendar>().HasIndex(x => x.Holiday).IsUnique();
        builder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();
        builder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Iva>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Neighborhood>().HasIndex(x => new {x.CityId, x.Name }).IsUnique();
        builder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        builder.Entity<Statu>().HasIndex(x => new {x.Name,x.Nivel }).IsUnique();


        builder.Entity<BudgetCourse>().HasIndex(x => new { x.ValidityId,x.CourseProgramLotId,x.StartDate }).IsUnique();
        builder.Entity<BudgetCourse>().ToTable(b=>b.HasCheckConstraint("CK_EndDateGreaterthanInitial", "[EndDate] > [StartDate]"));
        builder.Entity<BudgetType>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Classe>().HasIndex(x =>x.Code).IsUnique();
        builder.Entity<Classe>().HasIndex(x =>new {x.FamilyId, x.Code, x.Name }).IsUnique();

        //builder.Entity<Course>().HasMany(c=>c.CourseProgramLots).WithOne(x=>x.Course).HasForeignKey(x=>x.CourseId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Course>().HasIndex(x =>x.Name).IsUnique();

        builder.Entity<CourseProgramLot>().HasIndex(x =>new { x.CourseId,x.ProgramLotId }).IsUnique();
        
        builder.Entity<Family>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<Family>().HasIndex(x => new {x.SegmentId, x.Code, x.Name }).IsUnique();
        builder.Entity<InvProgram>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Lot>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Product>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<Product>().HasIndex(x => new {x.ClasseId, x.Name }).IsUnique();
        builder.Entity<ProductCurrentValue>().HasIndex(x => new {x.ValidityId, x.ProductId }).IsUnique();
        builder.Entity<ProgramLot>().HasIndex(x => new {x.ProgramId,x.LotId }).IsUnique();
        builder.Entity<Segment>().HasIndex(x => x.Code).IsUnique();
        builder.Entity<Segment>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<UnitMeasurement>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Validity>().HasIndex(x => x.Value).IsUnique();      

        DisableCascadingDelete(builder);

    }
    private void DisableCascadingDelete(ModelBuilder builder)
    {
        var relatioships = builder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys());

        foreach (var relationship in relatioships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

}
