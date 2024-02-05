﻿using System;
using HelloJobFinal.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels.Company
{
    public record UpdateCompanyVm(IFormFile? Photo, string ImageUrl, string Email, string AppUserId, string Status);

}

