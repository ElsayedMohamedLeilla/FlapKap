using FlapKap.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlapKap.BusinessLogic
{
    public static class StartupSetup
    {
        public static void ConfigureBusinessLogic(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
             .Where(p => p.Name.EndsWith("BL") && !p.IsInterface);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterfaces(false).FirstOrDefault(i => i.Name.EndsWith("BL"));
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
