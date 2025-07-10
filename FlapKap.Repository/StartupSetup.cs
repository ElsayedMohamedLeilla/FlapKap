using FlapKap.Contract.Repository;
using FlapKap.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlapKap.Repository
{
    public static class StartupSetup
    {

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            var types = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(p => p.Name.EndsWith("Repository") && !p.IsInterface);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces(false).FirstOrDefault(i => i.Name.EndsWith("Repository"));
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
                else
                {
                    services.AddScoped(type);
                }
            }

        }

    }
}
