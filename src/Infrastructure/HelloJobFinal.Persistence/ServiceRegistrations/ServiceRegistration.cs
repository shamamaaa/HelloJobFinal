using System;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Implementations;
using HelloJobFinal.Persistence.DAL;
using HelloJobFinal.Persistence.Implementations.Repositories;
using HelloJobFinal.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelloJobFinal.Persistence.ServiceRegistrations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBaseCategoryService, BaseCategoryService>();
            services.AddScoped<ICategoryItemService, CategoryItemService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICvService, CvService>();
            services.AddScoped<ICvWishlistService, CvWishlistService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IExperienceService, ExperienceService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IVacancyService, VacancyService>();
            services.AddScoped<IVacancyWishlistService, VacancyWishlistService>();
            services.AddScoped<IWorkingHourService, WorkingHourService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IBaseCategoryRepository, BaseCategoryRepository>();
            services.AddScoped<ICategoryItemRepository, CategoryItemRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICvRepository, CvRepository>();
            services.AddScoped<ICvRequestRepository, CvRequestRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();
            services.AddScoped<IVacancyRequestRepository, VacancyRequestRepository>();
            services.AddScoped<IWorkingHourRepository, WorkingHourRepository>();

            services.AddScoped<AppDbContextInitializer>();

            return services;
        }
    }

}

