using System;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Infrastructure.Implementations
{
    public class LayoutService
	{
        private readonly ISettingRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(ISettingRepository repository, UserManager<AppUser> userManager, IMapper mapper)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _repository.GetAll(false).ToDictionaryAsync(p => p.Key, p => p.Value);
            return settings;
        }
        public async Task<GetAppUserVM> GetByIdUserAsync(string id)
        {
            GetAppUserVM user = _mapper.Map<GetAppUserVM>(await _userManager.FindByIdAsync(id));
            return user;
        }
    }
}

