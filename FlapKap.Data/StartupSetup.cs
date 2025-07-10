using FlapKap.Data.UnitOfWork;
using FlapKap.Models.Context;
using Microsoft.Extensions.DependencyInjection;

namespace FlapKap.Data
{
    public static class StartupSetup
    {
        public static void ConfigureRepositoryContainer(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDBContext>();
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, UnitOfWork<ApplicationDBContext>>();
            services.AddScoped<RequestInfo>();

        }
    }
}
