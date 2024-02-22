using System;
namespace HelloJobFinal.Application.ViewModels
{
	public record CvWishlistItemVm
	{
        public int Id { get; init; }
        public string ImageUrl { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Position { get; init; }
        public int? Salary { get; init; }
        public IncludWorkingHourVm IncludWorkingHourVm { get; init; }
        public int WorkingHourId { get; init; }
        public IncludeExperienceVm IncludeExperienceVm { get; init; }
        public int ExperienceId { get; init; }

    }
}

