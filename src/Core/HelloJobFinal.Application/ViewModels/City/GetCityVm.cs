﻿using System;

namespace HelloJobFinal.Application.ViewModels
{
    public record GetCityVm
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public List<IncludeCvVm> Cvs { get; init; }
        public List<IncludeVacancyVm> Vacancies { get; init; }


    }
}

