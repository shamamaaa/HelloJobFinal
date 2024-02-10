using System;
using System.Linq.Expressions;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Application.ViewModels.City;
using HelloJobFinal.Application.ViewModels.Company;
using HelloJobFinal.Application.ViewModels.Cv;
using HelloJobFinal.Application.ViewModels.Education;
using HelloJobFinal.Application.ViewModels.Experience;
using HelloJobFinal.Application.ViewModels.WorkingHour;
using HelloJobFinal.Domain.Entities;
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
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _educationRepository.CheckUniqueAsync(x => x.Id == create.EducationId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _experienceRepository.CheckUniqueAsync(x => x.Id == create.ExperienceId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _workingHourRepository.CheckUniqueAsync(x => x.Id == create.WorkingHourId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!await _categoryItemRepository.CheckUniqueAsync(x => x.Id == create.CategoryId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if (!create.CvFile.ValidateTypeCVFile("image/", ".docx", ".pdf"))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CvFile", "File type is not valid.");
                return false;
            }
            if (!create.CvFile.ValidataSize())
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("CvFile", "Max file 5Mb.");
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
                model.AddModelError("Photo", "Max file 5Mb.");
                return false;
            }
            Cv item = _mapper.Map<Cv>(create);
            item.CvFile = await create.CvFile.CreateFileAsync(_env.WebRootPath, "assets", "User", "Cvs");
            item.ImageUrl = await create.CvFile.CreateFileAsync(_env.WebRootPath, "assets", "User");

            //item.CreatedBy = _http.HttpContext.User.Identity.Name;

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }


        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemCvVm>> GetAllWhereAsync(int take, int page = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ItemCvVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Cv, object>>? orderExpression, int page = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<GetCvVm> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationVm<ItemCvVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationVm<ItemCvVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            throw new NotImplementedException();
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

            UpdatePopulateDropdowns(update);
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
            if (!await _categoryItemRepository.CheckUniqueAsync(x => x.Id == update.CategoryId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("CorporateId", "Corporate not found");
                return false;
            }
            if(update.CvFile != null)
            {
                if (!update.CvFile.ValidateTypeCVFile("image/", ".docx", ".pdf"))
                {
                    await UpdatePopulateDropdowns(update);
                    model.AddModelError("CvFile", "File type is not valid.");
                    return false;
                }
                if (!update.CvFile.ValidataSize())
                {
                    await UpdatePopulateDropdowns(update);
                    model.AddModelError("CvFile", "Max file 5Mb.");
                    return false;
                }
                item.CvFile = await update.CvFile.CreateFileAsync(_env.WebRootPath, "assets", "User", "Cvs");
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
                item.ImageUrl = await update.CvFile.CreateFileAsync(_env.WebRootPath, "assets", "User");
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
            update.CategoryItems = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync());
            update.Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync());
            update.Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync());
            update.Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync());
            update.WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync());
        }
        public async Task CreatePopulateDropdowns(CreateCvVm create)
        {
            create.CategoryItems = _mapper.Map<List<IncludeCategoryItemVm>>(await _categoryItemRepository.GetAll().ToListAsync());
            create.Cities = _mapper.Map<List<IncludeCityVm>>(await _cityRepository.GetAll().ToListAsync());
            create.Educations = _mapper.Map<List<IncludeEducationVm>>(await _educationRepository.GetAll().ToListAsync());
            create.Experiences = _mapper.Map<List<IncludeExperienceVm>>(await _experienceRepository.GetAll().ToListAsync());
            create.WorkingHours = _mapper.Map<List<IncludWorkingHourVm>>(await _workingHourRepository.GetAll().ToListAsync());
        }


    }
}

