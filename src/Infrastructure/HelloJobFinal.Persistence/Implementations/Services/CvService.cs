using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Domain.Enums;
using HelloJobFinal.Infrastructure.Exceptions;
using HelloJobFinal.Infrastructure.Implementations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class CvService : ICvService
	{
        private readonly IMapper _mapper;
        private readonly ICvRepository _repository;
        private readonly ICityRepository _cityRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IExperienceRepository _experienceRepository;
        private readonly IWorkingHourRepository _workingHourRepository;
        private readonly ICategoryItemRepository _categoryItemRepository;
        private readonly IHttpContextAccessor _http;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public CvService(IMapper mapper, ICvRepository repository, ICityRepository cityRepository, IEducationRepository educationRepository, IExperienceRepository experienceRepository, IWorkingHourRepository workingHourRepository, ICategoryItemRepository categoryItemRepository, IHttpContextAccessor http, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _cityRepository = cityRepository;
            _educationRepository = educationRepository;
            _experienceRepository = experienceRepository;
            _workingHourRepository = workingHourRepository;
            _categoryItemRepository = categoryItemRepository;
            _http = http;
            _env = env;
            _userManager = userManager;
        }

        public async Task<bool> CreateAsync(CreateCvVm create, ModelStateDictionary model)
        {
            if (!model.IsValid)
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
            if (!await _categoryItemRepository.CheckUniqueAsync(x => x.Id == create.CategoryItemId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CategoryItemId", "Category not found");
                return false;
            }
            if (!create.CvFile.ValidateTypeCVFile("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/pdf"))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CvFile", "File type is not valid.");
                return false;
            }
            if (!create.CvFile.ValidataSize(10))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CvFile", "Maximum size of Cv file must be 10 Mb.");
                return false;
            }
            if (!create.Photo.ValidateType())
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("Photo", "File type is not valid.");
                return false;
            }
            if (!create.Photo.ValidataSize())
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("Photo", "Maximum size of photo must be 5 Mb.");
                return false;
            }

            Cv item = _mapper.Map<Cv>(create);

            item.CvFile = await create.CvFile.CreateFileAsync(_env.WebRootPath, "assets", "images","User", "CVs");
            item.ImageUrl = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "User");
            item.Status = Status.New.ToString();
            item.AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }


        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            Cv item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            item.CvFile.DeleteFile(_env.WebRootPath, "assets", "User", "CVs");
            item.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "User");

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemCvVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes ={
                $"{nameof(Cv.Experience)}",
                $"{nameof(Cv.Education)}",
                $"{nameof(Cv.City)}",
                $"{nameof(Cv.WorkingHour)}",
                $"{nameof(Cv.CategoryItem)}" };
            ICollection<Cv> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCvVm> vMs = _mapper.Map<ICollection<ItemCvVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemCvVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Cv, object>>? orderExpression, int page = 1)
        {
            string[] includes ={
                $"{nameof(Cv.Experience)}",
                $"{nameof(Cv.Education)}",
                $"{nameof(Cv.City)}",
                $"{nameof(Cv.WorkingHour)}",
                $"{nameof(Cv.CategoryItem)}" };
            ICollection<Cv> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCvVm> vMs = _mapper.Map<ICollection<ItemCvVm>>(items);

            return vMs;
        }

        public async Task<GetCvVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("You sent wrong request, please include valid input.");
            string[] includes ={
                $"{nameof(Cv.Experience)}",
                $"{nameof(Cv.Education)}",
                $"{nameof(Cv.City)}",
                $"{nameof(Cv.WorkingHour)}",
                $"{nameof(Cv.CategoryItem)}" };

            Cv item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException("Your request was not found");

            GetCvVm get = _mapper.Map<GetCvVm>(item);

            return get;
        }

        public async Task<PaginationVm<CvFilterVM>> GetDeleteFilteredAsync(string? search, int take, int page, int order,
            int? CategoryItemId, int? cityId, int? educationId, int? experienceId, int? workingHourId)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            string[] includes ={
                $"{nameof(Cv.Experience)}",
                $"{nameof(Cv.Education)}",
                $"{nameof(Cv.City)}",
                $"{nameof(Cv.WorkingHour)}",
                $"{nameof(Cv.CategoryItem)}" };
            double count = await _repository.CountAsync();

            ICollection<Cv> items = new List<Cv>();
            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                        x => x.Name, false, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, false, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                        x => x.Name, true, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, true, true, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            CvFilterVM filtered = new CvFilterVM
            {
                Cvs = _mapper.Map<List<ItemCvVm>>(items),
                Categories = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync()),
                Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync()),
                Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync()),
                Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync()),
                WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync())
            };
            PaginationVm<CvFilterVM> pagination = new PaginationVm<CvFilterVM>
            {
                Take = take,
                Search = search,
                Order = order,
                CategoryId = CategoryItemId,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Item = filtered
            };

            return pagination;
        }

        public async Task<PaginationVm<CvFilterVM>> GetFilteredAsync(string? search, int take, int page, int order,
            int? CategoryItemId, int? cityId, int? educationId,int? experienceId, int? workingHourId)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            string[] includes ={
                $"{nameof(Cv.Experience)}",
                $"{nameof(Cv.Education)}",
                $"{nameof(Cv.City)}",
                $"{nameof(Cv.WorkingHour)}",
                $"{nameof(Cv.CategoryItem)}" };
            double count = await _repository.CountAsync();

            ICollection<Cv> items = new List<Cv>();
            switch (order)
            {
                case 1:
                    items = await _repository
                    .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                        x => x.Name, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                        x => x.Name, true, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true),
                      x => x.CreatedAt, true, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
            }

            CvFilterVM filtered = new CvFilterVM
            {
                Cvs = _mapper.Map<List<ItemCvVm>>(items),
                Categories = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync()),
                Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync()),
                Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync()),
                Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync()),
                WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync())
            };
            PaginationVm<CvFilterVM> pagination = new PaginationVm<CvFilterVM>
            {
                Take = take,
                Search = search,
                Order = order,
                CategoryId = CategoryItemId,
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / take),
                Item = filtered
            };

            return pagination;
        }

        public async Task ReverseSoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            item.IsDeleted = false;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            item.IsDeleted = true;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateCvVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            UpdateCvVm update = _mapper.Map<UpdateCvVm>(item);

            await UpdatePopulateDropdowns(update);
            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateCvVm update, ModelStateDictionary model)
        {
            if (!model.IsValid)
            {
                await UpdatePopulateDropdowns(update);
                return false;
            }
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv item = await _repository.GetByIdAsync(id);
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
            if (!await _categoryItemRepository.CheckUniqueAsync(x => x.Id == update.CategoryItemId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if(update.CvUFile != null)
            {
                if (!update.CvUFile.ValidateTypeCVFile("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/pdf"))
                {
                    await UpdatePopulateDropdowns(update);
                    model.AddModelError("CvFile", "File type is not valid.");
                    return false;
                }
                if (!update.CvUFile.ValidataSize())
                {
                    await UpdatePopulateDropdowns(update);
                    model.AddModelError("CvFile", "Max file 5Mb.");
                    return false;
                }
                item.CvFile.DeleteFile(_env.WebRootPath, "assets", "User", "Cvs");
                item.CvFile = await update.CvUFile.CreateFileAsync(_env.WebRootPath, "assets", "User", "Cvs");
            }
            if(update.Photo != null)
            {
                if (!update.Photo.ValidateType())
                {
                    await UpdatePopulateDropdowns(update);
                    model.AddModelError("Photo", "File type is not valid.");
                    return false;
                }
                if (!update.Photo.ValidataSize())
                {
                    await UpdatePopulateDropdowns(update);
                    model.AddModelError("Photo", "Max file 5Mb.");
                    return false;
                }
                item.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "User", "Cvs");
                item.ImageUrl = await update.CvUFile.CreateFileAsync(_env.WebRootPath, "assets", "User");
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateCvVm, Cv>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                    .ForMember(dest => dest.CvFile, opt => opt.Ignore());
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            //item.CreatedBy = _http.HttpContext.User.Identity.Name;

            _repository.Update(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task UpdatePopulateDropdowns(UpdateCvVm update)
        {
            update.CategoryItems = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll(false, $"{nameof(CategoryItem.BaseCategory)}").ToListAsync());
            update.Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync());
            update.Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync());
            update.Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync());
            update.WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync());
        }
        public async Task CreatePopulateDropdowns(CreateCvVm create)
        {
            create.CategoryItems = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll(false, $"{nameof(CategoryItem.BaseCategory)}").ToListAsync());
            create.Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync());
            create.Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync());
            create.Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync());
            create.WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync());
        }


    }
}

