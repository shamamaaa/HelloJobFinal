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
    public class VacancyService : IVacancyService
    {
        private readonly IMapper _mapper;
        private readonly IVacancyRepository _repository;
        private readonly ICityRepository _cityRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IExperienceRepository _experienceRepository;
        private readonly IWorkingHourRepository _workingHourRepository;
        private readonly ICategoryItemRepository _categoryItemRepository;

        public VacancyService(IMapper mapper, IVacancyRepository repository, ICityRepository cityRepository,
            IEducationRepository educationRepository, IExperienceRepository experienceRepository,
            IWorkingHourRepository workingHourRepository, ICategoryItemRepository categoryItemRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _cityRepository = cityRepository;
            _educationRepository = educationRepository;
            _experienceRepository = experienceRepository;
            _workingHourRepository = workingHourRepository;
            _categoryItemRepository = categoryItemRepository;
        }

        public async Task CreatePopulateDropdowns(CreateVacancyVm create)
        {
            create.CategoryItems = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync());
            create.Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync());
            create.Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync());
            create.Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync());
            create.WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync());

        }
        public async Task UpdatePopulateDropdowns(UpdateVacancyVm update)
        {
            update.CategoryItems = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync());
            update.Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync());
            update.Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync());
            update.Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync());
            update.WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync());
        }
        public async Task<bool> CreateAsync(CreateVacancyVm create, ModelStateDictionary model)
        {
            if(!model.IsValid)
            {
                await CreatePopulateDropdowns(create);
                return false;
            }
            if (!await _cityRepository.CheckUniqueAsync(x => x.Id == create.CityId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CityId", "City not found");
                return false;
            }
            if (!await _educationRepository.CheckUniqueAsync(x => x.Id == create.EducationId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("EducationId", "Education not found");
                return false;
            }
            if (!await _experienceRepository.CheckUniqueAsync(x => x.Id == create.ExperienceId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("ExperienceId", "Experience not found");
                return false;
            }
            if (!await _workingHourRepository.CheckUniqueAsync(x => x.Id == create.WorkingHourId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("WorkingHourId", "Working-hour not found");
                return false;
            }
            if (!await _categoryItemRepository.CheckUniqueAsync(x => x.Id == create.CategoryId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CategoryId", "Category not found");
                return false;
            }

            Vacancy item = _mapper.Map<Vacancy>(create);

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            Vacancy item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemVacancyVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes ={
                $"{nameof(Vacancy.Experience)}",
                $"{nameof(Vacancy.Education)}",
                $"{nameof(Vacancy.City)}",
                $"{nameof(Vacancy.WorkingHour)}",
                $"{nameof(Vacancy.CategoryItem)}" };
            ICollection<Vacancy> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemVacancyVm> vMs = _mapper.Map<ICollection<ItemVacancyVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemVacancyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Vacancy, object>>? orderExpression, int page = 1)
        {
            string[] includes ={
                $"{nameof(Vacancy.Experience)}",
                $"{nameof(Vacancy.Education)}",
                $"{nameof(Vacancy.City)}",
                $"{nameof(Vacancy.WorkingHour)}",
                $"{nameof(Vacancy.CategoryItem)}" };
            ICollection<Vacancy> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemVacancyVm> vMs = _mapper.Map<ICollection<ItemVacancyVm>>(items);

            return vMs;
        }

        public async Task<GetVacancyVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            string[] includes ={
                $"{nameof(Vacancy.Experience)}",
                $"{nameof(Vacancy.Education)}",
                $"{nameof(Vacancy.City)}",
                $"{nameof(Vacancy.WorkingHour)}",
                $"{nameof(Vacancy.CategoryItem)}" };

            Vacancy item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException("Your request was not found");

            GetVacancyVm get = _mapper.Map<GetVacancyVm>(item);

            return get;
        }

        public async Task<PaginationVm<VacancyFilterVM>> GetDeleteFilteredAsync(string? search, int take, int page, int order,
              int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            string[] includes ={
                $"{nameof(Vacancy.Experience)}",
                $"{nameof(Vacancy.Education)}",
                $"{nameof(Vacancy.City)}",
                $"{nameof(Vacancy.WorkingHour)}",
                $"{nameof(Vacancy.CategoryItem)}" };
            double count = await _repository.CountAsync();

            ICollection<Vacancy> items = new List<Vacancy>();
            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                        x => x.Position, false, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, false, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                        x => x.Position, true, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, true, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            VacancyFilterVM filtered = new VacancyFilterVM
            {
                Vacancys = _mapper.Map<List<ItemVacancyVm>>(items),
                Categories = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync()),
                Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync()),
                Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync()),
                Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync()),
                WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync())
            };
            PaginationVm<VacancyFilterVM> pagination = new PaginationVm<VacancyFilterVM>
            {
                Take = take,
                Search = search,
                Order = order,
                CategoryId = categoryId,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Item = filtered
            };

            return pagination;
        }

        public async Task<PaginationVm<VacancyFilterVM>> GetFilteredAsync(string? search, int take, int page, int order,
              int? categoryId, int? cityId, int? educationId, int? experienceId, int? workingHourId)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            string[] includes ={
                $"{nameof(Vacancy.Experience)}",
                $"{nameof(Vacancy.Education)}",
                $"{nameof(Vacancy.City)}",
                $"{nameof(Vacancy.WorkingHour)}",
                $"{nameof(Vacancy.CategoryItem)}" };
            double count = await _repository.CountAsync();

            ICollection<Vacancy> items = new List<Vacancy>();
            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                        x => x.CategoryItem.Name, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                        x => x.Position, true, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => (categoryId != null ? x.CategoryId == categoryId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Position.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, true, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }
            VacancyFilterVM filtered = new VacancyFilterVM
            {
                Vacancys = _mapper.Map<List<ItemVacancyVm>>(items),
                Categories = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync()),
                Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync()),
                Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync()),
                Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync()),
                WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync())
            };
            PaginationVm<VacancyFilterVM> pagination = new PaginationVm<VacancyFilterVM>
            {
                Take = take,
                Search = search,
                Order = order,
                CategoryId = categoryId,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Item = filtered
            };

            return pagination;
        }

        public async Task ReverseSoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Vacancy item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            item.IsDeleted = false;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Vacancy item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            item.IsDeleted = true;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateVacancyVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Vacancy item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            UpdateVacancyVm update = _mapper.Map<UpdateVacancyVm>(item);

            UpdatePopulateDropdowns(update);
            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateVacancyVm update, ModelStateDictionary model)
        {
            if (!model.IsValid)
            {
                await UpdatePopulateDropdowns(update);
                return false;
            }
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Vacancy item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            if (!await _cityRepository.CheckUniqueAsync(x => x.Id == update.CityId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _educationRepository.CheckUniqueAsync(x => x.Id == update.EducationId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _experienceRepository.CheckUniqueAsync(x => x.Id == update.ExperienceId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _workingHourRepository.CheckUniqueAsync(x => x.Id == update.WorkingHourId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _categoryItemRepository.CheckUniqueAsync(x => x.Id == update.CategoryId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateVacancyVm, Vacancy>();
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            //item.CreatedBy = _http.HttpContext.User.Identity.Name;

            _repository.Update(item);
            await _repository.SaveChanceAsync();

            return true;
        }
    }
}

