using Microsoft.EntityFrameworkCore;
using TTManagement.Entities;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TTManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        // Task Configuration
        modelBuilder.Entity<ProjectTask>()
            .Property(t => t.Status)
            .HasConversion<string>();

        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.AssignedToUser)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.CreatedByUser)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.Team)
            .WithMany(tm => tm.Tasks)
            .HasForeignKey(t => t.TeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seeding
        SeedUsers(modelBuilder);
    }

    private void SeedUsers(ModelBuilder modelBuilder)
    {
        var admin = new User
        {
            Id = 1,
            FullName = "Admin User",
            Email = "admin@demo.com",
            Role = UserRole.Admin,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
        };

        var manager = new User
        {
            Id = 2,
            FullName = "Manager User",
            Email = "manager@demo.com",
            Role = UserRole.Manager,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager123!")
        };

        var employee = new User
        {
            Id = 3,
            FullName = "Employee User",
            Email = "employee@demo.com",
            Role = UserRole.Employee,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee123!")
        };

        modelBuilder.Entity<User>().HasData(admin, manager, employee);
    }
}
