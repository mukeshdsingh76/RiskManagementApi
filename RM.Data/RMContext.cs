using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RM.Data.Models;

namespace RM.Data
{
  public class RMContext : IdentityDbContext<User>
  {
    public RMContext(DbContextOptions option) : base(option)
    {
    }

    //public DbSet<Role> Role { get; set; }
    //public DbSet<User> User { get; set; }
    public DbSet<Project> Project { get; set; }

    public DbSet<Risk> Risk { get; set; }
    public DbSet<ProjectStatus> ProjectStatus { get; set; }
    public DbSet<RiskStatus> RiskStatus { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      //builder.Entity<Project>()
      //            .Property(p => p.ProjectCode)
      //            .HasColumnName("ProjectCode")
      //            .HasColumnType("string");

      builder.Entity<ProjectStatus>()
        .HasIndex(_ => _.Title)
        .HasName("UK_ProjectStatus_Title")
        .IsUnique();

      builder.Entity<RiskStatus>()
       .HasIndex(_ => _.Title)
       .HasName("UK_RiskStatus_Title")
       .IsUnique();
    }
  }
}