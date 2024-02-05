﻿using System;
using HelloJobFinal.Application.ViewModels.Vacancy;

namespace HelloJobFinal.Application.ViewModels.Company
{
    public record IncludeCompanyVm(int Id, string ImageUrl, string Email, string AppUserId, string Status, ICollection<IncludeVacancyVm> IncludeVacancies);

}
