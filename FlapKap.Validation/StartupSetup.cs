using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlapKap.Validation
{
    public static class StartupSetup
    {
        public static void ConfigureBLValidation(this IServiceCollection services)
        {
            var bLValidationTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(p => p.Name.EndsWith("BLValidation") && !p.IsInterface);

            foreach (var type in bLValidationTypes)
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name.EndsWith("BLValidation"));
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
