
using HelloJobFinal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace HelloJobFinal.Persistence.Common
{
    internal static class GlobalQueryFilter
    {
        public static void ApplyQuery<T>(ModelBuilder builder) where T : BaseEntity, new()
        {
            builder.Entity<T>().HasQueryFilter(x => x.IsDeleted == false);
        }

        public static void ApplyQueryFilters(this ModelBuilder builder)
        {
            ApplyQuery<BaseCategory>(builder);
            ApplyQuery<CategoryItem>(builder);
            ApplyQuery<City>(builder);
            ApplyQuery<Company>(builder);
            ApplyQuery<Cv>(builder);
            ApplyQuery<Education>(builder);
            ApplyQuery<Experience>(builder);
            ApplyQuery<Requirement>(builder);
            ApplyQuery<Setting>(builder);
            ApplyQuery<Vacancy>(builder);
            ApplyQuery<WishListItem>(builder);
            ApplyQuery<WorkInfo>(builder);
            ApplyQuery<WorkingHour>(builder);

        }

    }
}

