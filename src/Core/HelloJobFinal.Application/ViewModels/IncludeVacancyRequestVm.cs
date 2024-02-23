using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
	public record IncludeVacancyRequestVm
	{
        public string AppUserId { get; init; }
        public IncludeAppUserVm AppUser { get; set; }

        public int VacancyId { get; set; }

        public string Status { get; set; }

    }
}

