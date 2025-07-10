using FlapKap.Domain.Entities.User;
using Microsoft.Extensions.DependencyInjection;

namespace FlapKap.Data
{
    public class SeedDB
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDBContext>();
            context.Database.EnsureCreated();

            var rolesToSeed = new List<Role>
            {
                new() { Name = "Seller", NormalizedName = "Seller" , ConcurrencyStamp =null},
                new() { Name = "Buyer", NormalizedName = "Buyer", ConcurrencyStamp =null},
            };
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(rolesToSeed);
                context.SaveChanges();
            }

        }
    }
}