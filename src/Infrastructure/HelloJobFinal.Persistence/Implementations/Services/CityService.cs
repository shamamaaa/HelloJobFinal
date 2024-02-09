using System.Linq.Expressions;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class CityService : ICityService
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _repository;

        public CityService(IMapper mapper, ICityRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> CreateAsync(CreateCityVm create, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name == create.Name))
            {
                model.AddModelError("Name", "City already exists.");
                return false;
            }

            City item = _mapper.Map<City>(create);

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };
            City item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException("Your request was not found");

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemCityVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };

            ICollection<City> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCityVm> vMs = _mapper.Map<ICollection<ItemCityVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemCityVm>> GetAllWhereByOrderAsync(int take, Expression<Func<City, object>>? orderExpression, int page = 1)
        {
            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };

            ICollection<City> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCityVm> vMs = _mapper.Map<ICollection<ItemCityVm>>(items);

            return vMs;
        }

        public async Task<GetCityVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };

            City item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException("Your request was not found");

            GetCityVm get = _mapper.Map<GetCityVm>(item);

            return get;
        }

        public async Task<PaginationVm<ItemCityVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            if (order <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");

            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<City> items = new List<City>();

            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => x.IsDeleted == true && !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, false, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(expression: x => x.IsDeleted == true && !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                     orderException: x => x.CreatedAt, false, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => x.IsDeleted == true && !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, true, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => x.IsDeleted == true && !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreatedAt, true, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            ICollection<ItemCityVm> vMs = _mapper.Map<ICollection<ItemCityVm>>(items);

            PaginationVm<ItemCityVm> pagination = new PaginationVm<ItemCityVm>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = vMs
            };

            return pagination;
        }

        public async Task<PaginationVm<ItemCityVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            if (order <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");

            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<City> items = new List<City>();

            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreatedAt, false, false, skip: (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                        x => x.Name, true, false, (page - 1) * take, take: take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => !string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true,
                      x => x.CreatedAt, true, false, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();
                    break;
            }

            ICollection<ItemCityVm> vMs = _mapper.Map<ICollection<ItemCityVm>>(items);

            PaginationVm<ItemCityVm> pagination = new PaginationVm<ItemCityVm>
            {
                Take = take,
                Search = search,
                Order = order,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Items = vMs
            };

            return pagination;
        }

        public async Task ReverseSoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            City item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            item.IsDeleted = false;
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            City item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            item.IsDeleted = true;
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateCityVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            City item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            UpdateCityVm update = _mapper.Map<UpdateCityVm>(item);

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateCityVm update, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower().Trim() == update.Name.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Name", "City already exists.");
                return false;
            }
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            string[] includes = { $"{nameof(City.Cvs)}", $"{nameof(City.Vacancies)}" };

            City item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException("Your request was not found");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateCityVm, City>();
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            await _repository.SaveChanceAsync();

            return true;
        }
    }

}

