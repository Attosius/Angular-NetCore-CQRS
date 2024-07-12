using Microsoft.EntityFrameworkCore;

namespace PromomashInc.Server.Context;

public class BloggingContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Province> Provinces { get; set; }

    public string DbPath { get; }
    public BloggingContext() : base()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        var folder = Environment.CurrentDirectory;
        var path = Path.Combine(folder, "LocalDb");
        DbPath = Path.Join(path, "clients.db");
    }
    public BloggingContext(DbContextOptions options) : base(options)
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
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