using System;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IMapper _mapper;
        private readonly IExperienceRepository _repository;

        public ExperienceService(IMapper mapper, IExperienceRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> CreateAsync(CreateExperienceVm create, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name == create.Name))
            {
                model.AddModelError("Name", "Experience already exists.");
                return false;
            }

            Experience item = _mapper.Map<Experience>(create);

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };
            Experience item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemExperienceVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };

            ICollection<Experience> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemExperienceVm> vMs = _mapper.Map<ICollection<ItemExperienceVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemExperienceVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Experience, object>>? orderExpression, int page = 1)
        {
            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };

            ICollection<Experience> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemExperienceVm> vMs = _mapper.Map<ICollection<ItemExperienceVm>>(items);

            return vMs;
        }

        public async Task<GetExperienceVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };

            Experience item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException();

            GetExperienceVm get = _mapper.Map<GetExperienceVm>(item);

            return get;
        }

        public async Task<PaginationVm<ItemExperienceVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<Experience> items = new List<Experience>();

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

            ICollection<ItemExperienceVm> vMs = _mapper.Map<ICollection<ItemExperienceVm>>(items);

            PaginationVm<ItemExperienceVm> pagination = new PaginationVm<ItemExperienceVm>
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

        public async Task<PaginationVm<ItemExperienceVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<Experience> items = new List<Experience>();

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

            ICollection<ItemExperienceVm> vMs = _mapper.Map<ICollection<ItemExperienceVm>>(items);

            PaginationVm<ItemExperienceVm> pagination = new PaginationVm<ItemExperienceVm>
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
            if (id <= 0) throw new WrongRequestException();
            Experience item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = false;
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            Experience item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = true;
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateExperienceVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            Experience item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            UpdateExperienceVm update = _mapper.Map<UpdateExperienceVm>(item);

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateExperienceVm update, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower().Trim() == update.Name.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Name", "Experience already exists.");
                return false;
            }
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(Experience.Cvs)}", $"{nameof(Experience.Vacancies)}" };

            Experience item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateExperienceVm, Experience>();
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            await _repository.SaveChanceAsync();

            return true;
        }
    }

}

