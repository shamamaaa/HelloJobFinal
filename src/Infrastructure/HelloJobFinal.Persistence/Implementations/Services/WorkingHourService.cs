using System.Linq.Expressions;
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
    public class WorkingHourService : IWorkingHourService
    {
        private readonly IMapper _mapper;
        private readonly IWorkingHourRepository _repository;

        public WorkingHourService(IMapper mapper, IWorkingHourRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<bool> CreateAsync(CreateWorkingHourVm create, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name == create.Name))
            {
                model.AddModelError("Name", "Working Hour already exists.");
                return false;
            }

            WorkingHour item = _mapper.Map<WorkingHour>(create);

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };
            WorkingHour item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemWorkingHourVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };

            ICollection<WorkingHour> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemWorkingHourVm> vMs = _mapper.Map<ICollection<ItemWorkingHourVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemWorkingHourVm>> GetAllWhereByOrderAsync(int take, Expression<Func<WorkingHour, object>>? orderExpression, int page = 1)
        {
            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };

            ICollection<WorkingHour> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemWorkingHourVm> vMs = _mapper.Map<ICollection<ItemWorkingHourVm>>(items);

            return vMs;
        }

        public async Task<GetWorkingHourVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };

            WorkingHour item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException();

            GetWorkingHourVm get = _mapper.Map<GetWorkingHourVm>(item);

            return get;
        }

        public async Task<PaginationVm<ItemWorkingHourVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<WorkingHour> items = new List<WorkingHour>();

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

            ICollection<ItemWorkingHourVm> vMs = _mapper.Map<ICollection<ItemWorkingHourVm>>(items);

            PaginationVm<ItemWorkingHourVm> pagination = new PaginationVm<ItemWorkingHourVm>
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

        public async Task<PaginationVm<ItemWorkingHourVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<WorkingHour> items = new List<WorkingHour>();

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

            ICollection<ItemWorkingHourVm> vMs = _mapper.Map<ICollection<ItemWorkingHourVm>>(items);

            PaginationVm<ItemWorkingHourVm> pagination = new PaginationVm<ItemWorkingHourVm>
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
            WorkingHour item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = false;
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            WorkingHour item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = true;
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateWorkingHourVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            WorkingHour item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            UpdateWorkingHourVm update = _mapper.Map<UpdateWorkingHourVm>(item);

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateWorkingHourVm update, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower().Trim() == update.Name.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Name", "Working Hour already exists.");
                return false;
            }
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(WorkingHour.Cvs)}", $"{nameof(WorkingHour.Vacancies)}" };

            WorkingHour item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateWorkingHourVm, WorkingHour>();
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            await _repository.SaveChanceAsync();

            return true;
        }
    }

}

