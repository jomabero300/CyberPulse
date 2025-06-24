using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Neighborhood> Neighborhoods { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Statu> Status { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Admi");
        builder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();
        builder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Neighborhood>().HasIndex(x => new {x.CityId, x.Name }).IsUnique();
        builder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        builder.Entity<Statu>().HasIndex(x => x.Name).IsUnique();
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
