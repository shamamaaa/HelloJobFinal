using System;
namespace HelloJobFinal.Application.ViewModels
{
	public record VacancyWishlistItemVm
	{
        public int? VacancyId { get; set; }
        public int Id { get; init; }
        public string Position { get; init; }
        public int? Salary { get; init; }
        public DateTime CreatedAt { get; init; }

        public IncludeCategoryItemVm IncludeCategoryItem { get; init; }
        public int CategoryId { get; init; }
	}
}

