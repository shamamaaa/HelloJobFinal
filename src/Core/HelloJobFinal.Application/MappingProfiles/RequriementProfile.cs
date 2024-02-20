﻿using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
    internal class RequriementProfile : Profile
    {
        public RequriementProfile()
        {
            CreateMap<IncludeRequirement, Requirement>().ReverseMap();
        }
    }
}
