using Microsoft.EntityFrameworkCore;
using PromomashInc.DataAccess.Models;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace PromomashInc.DataAccess.Context;

public class UserDataContext : DbContext
{
    public static string GetDbPath()
    {
        var folder = Environment.CurrentDirectory;
        var path = Path.Combine(folder, "LocalDb");
        var dbPath = Path.Join(path, "clients.db");
        return dbPath;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Province> Provinces { get; set; }

    public string DbPath { get; }
    public UserDataContext() : base()
    {
        DbPath = GetDbPath();
    }

    public UserDataContext(DbContextOptions options) : base(options)
    {
        DbPath = GetDbPath();
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
            entity.HasOne(v => v.Country).WithMany().HasForeignKey(v => v.CountryCode);
            entity.HasOne(v => v.Province).WithMany().HasForeignKey(v => v.ProvinceCode);

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
            entity.HasOne(v => v.Country).WithMany().HasForeignKey(v => v.ParentCode);
        });
    }

}