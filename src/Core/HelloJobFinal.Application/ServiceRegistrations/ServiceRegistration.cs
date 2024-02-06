using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HelloJobFinal.Application.ServiceRegistrations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}

