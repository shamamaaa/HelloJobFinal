﻿using System;
using AutoMapper;
using HelloJobFinal.Application.ViewModels.Setting;
using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.MappingProfiles
{
	internal class SettingProfile : Profile
    {
        public SettingProfile()
		{
            CreateMap<ItemSettingVm, Setting>().ReverseMap();
        }
    }
}

