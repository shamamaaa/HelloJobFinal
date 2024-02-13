﻿using System;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record GetAppUserVM(string Id, string Name, string Surname, string UserName, string Img, string Email, string? PhoneSecond,
        string? Address, string? About, string? FaceLink, string? TwitLink, string? GoogleLink, string? LinkedLink, string? InstaLink)
    {
        public string? PhoneNumber { get; init; }
        public List<IncludeCompanyVm>? Companies { get; set; }  //Company daxilinden vakansiyalara el catir
        public List<IncludeCvVm>? Cvs { get; set; }  //Employee elan kimi bir nece cv yerelesdire bilermis.
        public List<IncludeCvVm>? WishListCvs { get; set; }
        public List<IncludeVacancyVm>? WishListVacancies { get; set; }
        public List<IncludeCvVm>? CvRequests { get; set; }
        public List<IncludeVacancyVm>? VacancyRequests { get; set; }
    }
}

