﻿using System;
using Microsoft.AspNetCore.Http;

namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateBaseCategoryVm(string Name, IFormFile? Photo, string ImageUrl);

}

