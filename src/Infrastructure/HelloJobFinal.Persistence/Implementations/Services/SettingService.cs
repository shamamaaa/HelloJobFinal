using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class SettingService : ISettingService
    {
        private readonly IMapper _mapper;
        private readonly ISettingRepository _repository;

        public SettingService(ISettingRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PaginationVm<ItemSettingVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            if (order <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");

            double count = await _repository.CountAsync();

            ICollection<Setting> items = new List<Setting>();

            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => x.IsDeleted == false && !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                        x => x.Key, false, false, (page - 1) * take, take, false).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(expression: x => x.IsDeleted == false && !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreatedAt, false, false, (page - 1) * take, take, false).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => x.IsDeleted == false && !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                        x => x.Key, true, false, (page - 1) * take, take, false).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => x.IsDeleted == false && !string.IsNullOrEmpty(search) ? x.Key.ToLower().Contains(search.ToLower()) : true,
                    x => x.CreatedAt, true, false, (page - 1) * take, take, false).ToListAsync();
                    break;
            }

            ICollection<ItemSettingVm> Vms = _mapper.Map<ICollection<ItemSettingVm>>(items);

            PaginationVm<ItemSettingVm> pagination = new PaginationVm<ItemSettingVm>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = Vms
            };
            return pagination;
        }

        public async Task<UpdateSettingVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            Setting item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            UpdateSettingVm update = new UpdateSettingVm { Key = item.Key, Value = item.Value };

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateSettingVm update, ModelStateDictionary model)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            Setting item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            if (await _repository.CheckUniqueAsync(x => x.Key.ToLower().Trim() == update.Key.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Key", "Key is exists");
                return false;
            }
            item.Value = update.Value;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
            return true;
        }
    }

}

