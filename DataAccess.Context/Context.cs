using Microsoft.EntityFrameworkCore;
using PromomashInc.DataAccess.Models;

namespace PromomashInc.DataAccess.Context;

public class UserDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Province> Provinces { get; set; }

    public string DbPath { get; }
    public UserDataContext() : base()
    {
        var folder = Environment.CurrentDirectory;
        var path = Path.Combine(folder, "LocalDb");
        DbPath = Path.Join(path, "clients.db");
    }

    public UserDataContext(DbContextOptions options) : base(options)
    {
        var folder = Environment.CurrentDirectory;
        var path = Path.Combine(folder, "LocalDb");
        DbPath = Path.Join(path, "clients.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");


            entity.HasKey(o => o.Id);
        });
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Countries");


            entity.HasKey(o => o.Code);
        });
        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("Provinces");


            entity.HasKey(o => o.Code);
        });
    }

}