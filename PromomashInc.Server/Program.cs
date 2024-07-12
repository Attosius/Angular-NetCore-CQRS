
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PromomashInc.Server.Context;

namespace PromomashInc.Server
{
    public class Program
    {
        public static void SetOptions(DbContextOptionsBuilder optionBuilder)
        {
            var folder = Environment.CurrentDirectory;
            var path = Path.Combine(folder, "LocalDb");
            var DbPath = Path.Join(path, "clients.db");

            optionBuilder
                .UseSqlite("Data Source=clients.db;Cache=Shared")
                .EnableSensitiveDataLogging();
            //.UseSqlServer(@"Server=.;Initial Catalog=Application;Persist Security Info=False;Integrated Security=True;MultipleActiveResultSets=False;Connection Timeout=180;");
        }
        private static void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<BloggingContext>(SetOptions);
            //services.AddAsyncInitializer<ApplicationDbContextInitializer>();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            ConfigureDbContext(builder.Services);

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            AddDb();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void AddDb()
        {

            using var db = new BloggingContext();

            // Note: This sample requires the database to be created before running.
            Console.WriteLine($"Database path: {db.DbPath}.");


            db.Provinces.RemoveRange(db.Provinces);
            db.Countries.RemoveRange(db.Countries);
            db.SaveChanges();

            // Create
            Console.WriteLine("Inserting countries");
            var countries = Enumerable.Range(1, 4).Select(index => new Country
            {
                Code = $"Country_{index}",
                DisplayText = $"Country {index}"
            }).ToList();

            var provinces = Enumerable.Range(1, 10).Select(index => new Province()
                {
                    Code = $"Province_{index}",
                    ParentCode = $"Country_{index % 4 + 1}",
                    DisplayText = $"Province {index} "
                })
                .ToList();

            db.Countries.AddRange(countries);
            db.Provinces.AddRange(provinces);
            db.SaveChanges();

            //// Read
            //Console.WriteLine("Querying for a blog");
            //var blog = db.Blogs
            //    .OrderBy(b => b.BlogId)
            //    .First();

            //// Update
            //Console.WriteLine("Updating the blog and adding a post");
            //blog.Url = "https://devblogs.microsoft.com/dotnet";
            //blog.Posts.Add(
            //    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
            //db.SaveChanges();

            //// Delete
            //Console.WriteLine("Delete the blog");
            //db.Remove(blog);
            //db.SaveChanges();
        }
    }
}
