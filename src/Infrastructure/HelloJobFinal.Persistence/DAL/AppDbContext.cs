﻿using System.Reflection;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Persistence.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.DAL
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
		{

		}

		public DbSet<BaseCategory> BaseCategories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Cv> Cvs { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<Experience> Experiences { get; set; }

        public DbSet<Requirement> Requirements { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Vacancy> Vacancies { get; set; }

		public DbSet<WorkInfo> WorkInfos { get; set; }

		public DbSet<WorkingHour> WorkingHours { get; set; }

        public DbSet<WishListCv> WishListCvs { get; set; }

        public DbSet<WishListVacancy> WishListVacancies { get; set; }

        public DbSet<CvRequest> CvRequests { get; set; }

        public DbSet<VacancyRequest> VacancyRequests { get; set; }




        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyQueryFilters();

        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //    base.OnModelCreating(modelBuilder);
        //}

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entities = ChangeTracker.Entries<BaseEntity>();
        //    foreach (var data in entities)
        //    {
        //        switch (data.State)
        //        {
        //            case EntityState.Modified:
        //                data.Entity.ModifiedAt = DateTime.Now;
        //                break;
        //            case EntityState.Added:
        //                data.Entity.CreatedAt = DateTime.Now;
        //                break;
        //        }
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
