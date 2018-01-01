using Microsoft.EntityFrameworkCore;
using RM.Data.Models;

namespace RM.Data
{
  public class RMContext : DbContext
  {
    public RMContext(DbContextOptions option) : base(option)
    {
    }

    public DbSet<Project> Project { get; set; }

    public DbSet<Risk> Risk { get; set; }
    public DbSet<ProjectStatus> ProjectStatus { get; set; }
    public DbSet<RiskStatus> RiskStatus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<ProjectStatus>()
        .HasIndex(_ => _.Title)
        .HasName("UK_ProjectStatus_Title")
        .IsUnique();

      modelBuilder.Entity<RiskStatus>()
       .HasIndex(_ => _.Title)
       .HasName("UK_RiskStatus_Title")
       .IsUnique();
    }
  }
}