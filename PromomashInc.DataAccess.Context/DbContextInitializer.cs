using Microsoft.EntityFrameworkCore;
using PromomashInc.DataAccess.Models;

namespace PromomashInc.DataAccess.Context
{

    public class DbInitializer
    {

        public static void Initialize()
        {
            using var context = new UserDataContext();
            context.Database.Migrate();
            SeedDictionaries(context);
        }

        private static void SeedDictionaries(UserDataContext context)
        {

            Console.WriteLine($"Database path: {context.DbPath}.");


            context.Users.RemoveRange(context.Users);
            context.Provinces.RemoveRange(context.Provinces);
            context.Countries.RemoveRange(context.Countries);
            context.SaveChanges();

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

            context.Countries.AddRange(countries);
            context.Provinces.AddRange(provinces);
            context.SaveChanges();
            Console.WriteLine("SeedDictionaries done");
        }
    }
}