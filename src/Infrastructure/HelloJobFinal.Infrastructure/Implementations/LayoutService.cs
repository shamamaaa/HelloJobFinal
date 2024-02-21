using System;
using HelloJobFinal.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Infrastructure.Implementations
{
    public class LayoutService
	{
        private readonly ISettingRepository _repository;

        public LayoutService(ISettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _repository.GetAll(false).ToDictionaryAsync(p => p.Key, p => p.Value);
            return settings;
        }
    }
}

