using Microsoft.EntityFrameworkCore;
using RM.Data.Models;

namespace RM.Data
{
  public class RMContext : DbContext
  {
    public RMContext(DbContextOptions option) : base(option)
    {
    }

    public DbSet<Role> Role { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Project> Project { get; set; }
    public DbSet<Risk> Risk { get; set; }
    public DbSet<ProjectStatus> ProjectStatus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.Entity<Role>()
      //  .HasKey(_ => _.Id)
      //  .HasName("PrimaryKey_RoleId");

      //modelBuilder.Entity<User>()
      //  .HasKey(_ => _.Id)
      //  .HasName("PrimaryKey_UserId");

      //modelBuilder.Entity<ProjectStatus>()
      //  .HasKey(_ => _.Id)
      //  .HasName("PrimaryKey_ProjectStatusId");

      //modelBuilder.Entity<Project>()
      //  .HasKey(_ => _.Id)
      //  .HasName("PrimaryKey_ProjectId");

      //modelBuilder.Entity<Risk>()
      //  .HasKey(_ => _.Id)
      //  .HasName("PrimaryKey_RiskId");
    }
  }
}