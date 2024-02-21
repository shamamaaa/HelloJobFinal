using System;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HelloJobFinal.Infrastructure.ServiceRegistrations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<LayoutService>();

            return services;
        }
    }
}

