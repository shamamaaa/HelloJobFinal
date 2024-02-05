using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Cv
{
    public record UpdateCvVm(string Name, string Surname, string Email, DateTime Birthday, int Phone, string ImageUrl, IFormFile? Photo, string Position, int MinSalary, bool HasDriverLicense, DateTime FinishTime, string CvUrl ,IFormFile? CvFile, bool IsTimeOver, int ViewCount, int CityId, int EducationId, int ExperienceId, int WorkingHourId, int CategoryId, string AppUserId, string Status);

}

