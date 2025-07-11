using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Chip> Chips { get; set; }
    public DbSet<ChipHour> ChipHours { get; set; }
    public DbSet<ChipPoblation> ChipPoblations { get; set; }
    public DbSet<ChipProgram> ChipPrograms { get; set; }
    public DbSet<PriorityBet> PriorityBets { get; set; }
    public DbSet<TrainingProgram> TrainingPrograms { get; set; }
    public DbSet<TriningLevel> TriningLevels { get; set; }
    public DbSet<TypeOfPoblation> TypeOfPoblations { get; set; }
    public DbSet<TypeOfTraining> TypeOfTraining { get; set; }


    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Neighborhood> Neighborhoods { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Statu> Status { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Admi");

        builder.Entity<ChipHour>().HasIndex(x => x.ChipId).IsUnique();
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
        builder.Entity<Neighborhood>().HasIndex(x => new {x.CityId, x.Name }).IsUnique();
        builder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        builder.Entity<Statu>().HasIndex(x => new {x.Name,x.Nivel }).IsUnique();
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
