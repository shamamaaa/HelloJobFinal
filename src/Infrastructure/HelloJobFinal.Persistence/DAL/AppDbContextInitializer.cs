using System;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HelloJobFinal.Persistence.DAL
{
	public class AppDbContextInitializer
	{
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public AppDbContextInitializer(AppDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task InitializeDbContextAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task CreateUserRolesAsync()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }
        }

        public async Task InitializeAdminAsync()
        {
            AppUser admin = new AppUser
            {
                Name="Shamama",
                Surname="Guliyeva",
                Email = _configuration["AdminSettings:Email"],
                UserName = _configuration["AdminSettings:UserName"]
            };

            await _userManager.CreateAsync(admin, _configuration["AdminSettings:Password"]);
            await _userManager.AddToRoleAsync(admin, AdminRole.Admin.ToString());
        }
    }
}

