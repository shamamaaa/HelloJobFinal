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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        private readonly IEmailService _email;
        private readonly UserManager<AppUser> _userManager;

        public CvService(IMapper mapper, ICvRepository repository, ICityRepository cityRepository,
            IEducationRepository educationRepository, IExperienceRepository experienceRepository,
            IWorkingHourRepository workingHourRepository, ICategoryItemRepository categoryItemRepository,
            IHttpContextAccessor http, IWebHostEnvironment env, UserManager<AppUser> userManager, IEmailService email)
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
            _email = email;
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
            item.FinishTime = DateTime.Now.AddMonths(5);
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
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true)
                                && (x.FinishTime != null ? x.FinishTime <= DateTime.Now : true),
                        x => x.Name, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 2:
                    items = await _repository
                     .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true)
                                && (x.FinishTime != null ? x.FinishTime <= DateTime.Now : true),
                      x => x.CreatedAt, false, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 3:
                    items = await _repository
                    .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true)
                                && (x.FinishTime != null ? x.FinishTime <= DateTime.Now : true),
                                x => x.Name, true, false, (page - 1) * take, take, false, includes).ToListAsync();
                    break;
                case 4:
                    items = await _repository
                     .GetAllWhereByOrder(x => (CategoryItemId != null ? x.CategoryItemId == CategoryItemId : true)
                                && (cityId != null ? x.CityId == cityId : true)
                                && (educationId != null ? x.EducationId == educationId : true)
                                && (experienceId != null ? x.ExperienceId == experienceId : true)
                                && (workingHourId != null ? x.WorkingHourId == workingHourId : true)
                                && (!string.IsNullOrEmpty(search) ? x.Name.ToLower().Contains(search.ToLower()) : true)
                                && (x.FinishTime != null ? x.FinishTime <= DateTime.Now : true),
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
            item.FinishTime = DateTime.Now.AddMonths(5);
            _repository.Update(item);
            await _repository.SaveChanceAsync();

            return true;
        }
        public async Task<bool> AddCvRequestAsync(int id, ITempDataDictionary tempData)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv item = await _repository.GetByIdAsync(id, false, $"{nameof(Cv.AppUser)}");
            if (item == null) throw new NotFoundException("Your request was not found");
            if (await _repository.CheckUniqueCvRequestAsync(x => x.CvId == id && x.AppUserId == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                tempData["Message"] += $"<h6 class=\"text-danger\" style=\"margin-left: 100px; color: red;\"> Bu elana artıq müraciət edilib.</h6>";
                return false;
            }
            CvRequest request = new CvRequest
            {
                AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                CvId = item.Id,
                Status = Status.InProgress.ToString()
            };
            await _repository.AddCvRequest(request);
            await _repository.SaveChanceAsync();
            await _email.SendMailAsync(item.AppUser.Email, "Cv-ə müraciət", "<head>\n    <meta charset=\"UTF-8\" />\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\n    <style type=\"text/css\">\n        body {\n            margin: 0;\n            background: #FEFEFE;\n            color: #585858;\n        }\n\n        table {\n            font-size: 15px;\n            line-height: 23px;\n            max-width: 500px;\n            min-width: 460px;\n            text-align: center;\n        }\n\n        .table_inner {\n            min-width: 100% !important;\n        }\n\n        td {\n            font-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n            vertical-align: top;\n        }\n\n        .carpool_logo {\n            margin: 30px auto;\n        }\n\n        .dummy_row {\n            padding-top: 20px !important;\n        }\n\n        .section,\n        .sectionlike {\n            background: #C9F9E9;\n        }\n\n        .section {\n            padding: 0 20px;\n        }\n\n        .sectionlike {\n            padding-bottom: 10px;\n        }\n\n        .section_content {\n            width: 100%;\n            background: #fff;\n        }\n\n        .section_content_padded {\n            padding: 0 35px 40px;\n        }\n\n        .section_zag {\n            background: #F4FBF9;\n        }\n\n        .imageless_section {\n            padding-bottom: 20px;\n        }\n\n        img {\n            display: block;\n            margin: 0 auto;\n        }\n\n        .img_section {\n            width: 100%;\n            max-width: 500px;\n        }\n\n        .img_section_side_table {\n            width: 100% !important;\n        }\n\n        h1 {\n            font-size: 20px;\n            font-weight: 500;\n            margin-top: 40px;\n            margin-bottom: 0;\n        }\n\n        .near_title {\n            margin-top: 10px;\n        }\n\n        .last {\n            margin-bottom: 0;\n        }\n\n        a {\n            color: #63D3CD;\n            font-weight: 500;\n            word-break: break-word; /* Footer has long unsubscribe link */\n        }\n\n        .button {\n            display: block;\n            width: 100%;\n            max-width: 300px;\n            background: #20DA9C;\n            border-radius: 8px;\n            color: #fff;\n            font-size: 18px;\n            font-weight: normal; /* Resetting from a */\n            padding: 12px 0;\n            margin: 30px auto 0;\n            text-decoration: none;\n        }\n\n        small {\n            display: block;\n            width: 100%;\n            max-width: 330px;\n            margin: 14px auto 0;\n            font-size: 14px;\n        }\n\n        .signature {\n            padding: 20px;\n        }\n\n        .footer,\n        .footer_like {\n            background: #1FD99A;\n        }\n\n        .footer {\n            padding: 0 20px 30px;\n        }\n\n        .footer_content {\n            width: 100%;\n            text-align: center;\n            font-size: 12px;\n            line-height: initial;\n            color: #005750;\n        }\n\n            .footer_content a {\n                color: #005750;\n            }\n\n        .footer_item_image {\n            margin: 0 auto 10px;\n        }\n\n        .footer_item_caption {\n            margin: 0 auto;\n        }\n\n        .footer_legal {\n            padding: 20px 0 40px;\n            margin: 0;\n            font-size: 12px;\n            color: #A5A5A5;\n            line-height: 1.5;\n        }\n\n        .text_left {\n            text-align: left;\n        }\n\n        .text_right {\n            text-align: right;\n        }\n\n        .va {\n            vertical-align: middle;\n        }\n\n        .stats {\n            min-width: auto !important;\n            max-width: 370px;\n            margin: 30px auto 0;\n        }\n\n        .counter {\n            font-size: 22px;\n        }\n\n        .stats_counter {\n            width: 23%;\n        }\n\n        .stats_image {\n            width: 18%;\n            padding: 0 10px;\n        }\n\n        .stats_meta {\n            width: 59%;\n        }\n\n        .stats_spaced {\n            padding-top: 16px;\n        }\n\n        .walkthrough_spaced {\n            padding-top: 24px;\n        }\n\n        .walkthrough {\n            max-width: none;\n        }\n\n        .walkthrough_meta {\n            padding-left: 20px;\n        }\n\n        .table_checkmark {\n            padding-top: 30px;\n        }\n\n        .table_checkmark_item {\n            font-size: 15px;\n        }\n\n        .td_checkmark {\n            width: 24px;\n            padding: 7px 12px 0 0;\n        }\n\n        .padded_bottom {\n            padding-bottom: 40px;\n        }\n\n        .marginless {\n            margin: 0;\n        }\n\n        /* Restricting responsive for iOS Mail app only as Inbox/Gmail have render bugs */\n        @media only screen and (max-width: 480px) and (-webkit-min-device-pixel-ratio: 2) {\n            table {\n                min-width: auto !important;\n            }\n\n            .section_content_padded {\n                padding-right: 25px !important;\n                padding-left: 25px !important;\n            }\n\n            .counter {\n                font-size: 18px !important;\n            }\n        }\n    </style>\n</head>\n<body style=\"\tmargin: 0;\n\tbackground: #FEFEFE;\n\tcolor: #585858;\n\">\n    <!-- Preivew text -->\n    <span class=\"preheader\" style=\"display: none !important; visibility: hidden; opacity: 0; color: transparent; height: 0; width: 0;border-collapse: collapse;border: 0px;\"></span>\n    <!-- Carpool logo -->\n    <table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\">\n        <tbody>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <img src=\"https://media.licdn.com/dms/image/C4D0BAQFYfISsshjaNA/company-logo_200_200/0/1597744113257?e=2147483647&v=beta&t=mt3a8WUVVMk9isD7qn_DT_ssZfWlc8AIo7Re2Wux_PQ\" class=\"carpool_logo\" width=\"300\" height=\"300\" style=\"\tdisplay: block;\n\tmargin: 0 auto;\nmargin: 30px auto;border-radius:50%;object-fit:cover\">\n                </td>\n            </tr>\n            <!-- Header -->\n            <tr>\n                <td class=\"sectionlike imageless_section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n  background:  #1f9dff;\n  padding-bottom: 10px;\npadding-bottom: 20px;\"></td>\n            </tr>\n            <!-- Content -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  white;\n\">\n                        <tbody>\n                            <tr>\n                                <td class=\"section_content_padded\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 0 35px 40px;\">\n                                    <h1 style=\"\tfont-size: 20px;\n\tfont-weight: 500;\n\tmargin-top: 40px;\n\tmargin-bottom: 0;\n\">\n                                        Dəyərli HelloJob istifadəçisi, yerləşdirdiyiniz elanınıza müraciət edilib. Müraciəti gözdən keçirmək üçün hesabınıza daxil olub müraciətlər hissəsinə keçid edin.\n                                    </h1>\n                                    <p class=\"near_title last\" style=\"margin-top: 10px;margin-bottom: 0;\">Elan Yerləşdirdiyiniz üçün təşəkkürlər.</p>\n                                    <small style=\"\tdisplay: block;\n\twidth: 100%;\n\tmax-width: 330px;\n\tmargin: 14px auto 0;\n\tfont-size: 14px;\n\"></small>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Signature -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content section_zag\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  #1f9dff;\nbackground: #F4FBF9;\">\n                        <tbody>\n                            <tr>\n                                <td class=\"signature\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 20px;\">\n                                    <p class=\"marginless\" style=\"margin: 0;\"><br>HelloJob Team</p>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Footer -->\n            <tr>\n                <td class=\"section dummy_row\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\npadding-top: 20px !important;\"></td>\n            </tr>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n\tbackground: #fff;\n\">\n                    </table>\n                </td>\n            </tr>\n            <!-- Legal footer -->\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <p class=\"footer_legal\" style=\"\tpadding: 20px 0 40px;\n\tmargin: 0;\n\tfont-size: 12px;\n\tcolor: #A5A5A5;\n\tline-height: 1.5;\n\">\n                        Əgər bu mail sizə səhvən gəibsə zəhmət olmasa bizə bildirin<br><br>\n                        2023\n                        <br><br>\n\n                       HelloJob tərəfindən avtomatik göndırilmiş mail\n                    </p>\n                </td>\n            </tr>\n        </tbody>\n    </table>\n\n</body>", true);
            return true;
        }
        public async Task AcceptCvRequestAsync(int requestId)
        {
            if (requestId <= 0) throw new WrongRequestException("The request sent does not exist");
            CvRequest item = await _repository.GetByIdCvRequest(requestId);
            if (item == null) throw new NotFoundException("Your request was not found");
            AppUser company = await _userManager.FindByIdAsync(item.AppUserId);
            if (company == null) throw new NotFoundException("Your request was not found");
            item.Status = Status.Accepted.ToString();
            _repository.UpdateCvRequest(item);
            await _repository.SaveChanceAsync();
            await _email.SendMailAsync(company.Email, "Cv-ə müraciətinin cavabı", "<head>\n    <meta charset=\"UTF-8\" />\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\n    <style type=\"text/css\">\n        body {\n            margin: 0;\n            background: #FEFEFE;\n            color: #585858;\n        }\n\n        table {\n            font-size: 15px;\n            line-height: 23px;\n            max-width: 500px;\n            min-width: 460px;\n            text-align: center;\n        }\n\n        .table_inner {\n            min-width: 100% !important;\n        }\n\n        td {\n            font-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n            vertical-align: top;\n        }\n\n        .carpool_logo {\n            margin: 30px auto;\n        }\n\n        .dummy_row {\n            padding-top: 20px !important;\n        }\n\n        .section,\n        .sectionlike {\n            background: #C9F9E9;\n        }\n\n        .section {\n            padding: 0 20px;\n        }\n\n        .sectionlike {\n            padding-bottom: 10px;\n        }\n\n        .section_content {\n            width: 100%;\n            background: #fff;\n        }\n\n        .section_content_padded {\n            padding: 0 35px 40px;\n        }\n\n        .section_zag {\n            background: #F4FBF9;\n        }\n\n        .imageless_section {\n            padding-bottom: 20px;\n        }\n\n        img {\n            display: block;\n            margin: 0 auto;\n        }\n\n        .img_section {\n            width: 100%;\n            max-width: 500px;\n        }\n\n        .img_section_side_table {\n            width: 100% !important;\n        }\n\n        h1 {\n            font-size: 20px;\n            font-weight: 500;\n            margin-top: 40px;\n            margin-bottom: 0;\n        }\n\n        .near_title {\n            margin-top: 10px;\n        }\n\n        .last {\n            margin-bottom: 0;\n        }\n\n        a {\n            color: #63D3CD;\n            font-weight: 500;\n            word-break: break-word; /* Footer has long unsubscribe link */\n        }\n\n        .button {\n            display: block;\n            width: 100%;\n            max-width: 300px;\n            background: #20DA9C;\n            border-radius: 8px;\n            color: #fff;\n            font-size: 18px;\n            font-weight: normal; /* Resetting from a */\n            padding: 12px 0;\n            margin: 30px auto 0;\n            text-decoration: none;\n        }\n\n        small {\n            display: block;\n            width: 100%;\n            max-width: 330px;\n            margin: 14px auto 0;\n            font-size: 14px;\n        }\n\n        .signature {\n            padding: 20px;\n        }\n\n        .footer,\n        .footer_like {\n            background: #1FD99A;\n        }\n\n        .footer {\n            padding: 0 20px 30px;\n        }\n\n        .footer_content {\n            width: 100%;\n            text-align: center;\n            font-size: 12px;\n            line-height: initial;\n            color: #005750;\n        }\n\n            .footer_content a {\n                color: #005750;\n            }\n\n        .footer_item_image {\n            margin: 0 auto 10px;\n        }\n\n        .footer_item_caption {\n            margin: 0 auto;\n        }\n\n        .footer_legal {\n            padding: 20px 0 40px;\n            margin: 0;\n            font-size: 12px;\n            color: #A5A5A5;\n            line-height: 1.5;\n        }\n\n        .text_left {\n            text-align: left;\n        }\n\n        .text_right {\n            text-align: right;\n        }\n\n        .va {\n            vertical-align: middle;\n        }\n\n        .stats {\n            min-width: auto !important;\n            max-width: 370px;\n            margin: 30px auto 0;\n        }\n\n        .counter {\n            font-size: 22px;\n        }\n\n        .stats_counter {\n            width: 23%;\n        }\n\n        .stats_image {\n            width: 18%;\n            padding: 0 10px;\n        }\n\n        .stats_meta {\n            width: 59%;\n        }\n\n        .stats_spaced {\n            padding-top: 16px;\n        }\n\n        .walkthrough_spaced {\n            padding-top: 24px;\n        }\n\n        .walkthrough {\n            max-width: none;\n        }\n\n        .walkthrough_meta {\n            padding-left: 20px;\n        }\n\n        .table_checkmark {\n            padding-top: 30px;\n        }\n\n        .table_checkmark_item {\n            font-size: 15px;\n        }\n\n        .td_checkmark {\n            width: 24px;\n            padding: 7px 12px 0 0;\n        }\n\n        .padded_bottom {\n            padding-bottom: 40px;\n        }\n\n        .marginless {\n            margin: 0;\n        }\n\n        /* Restricting responsive for iOS Mail app only as Inbox/Gmail have render bugs */\n        @media only screen and (max-width: 480px) and (-webkit-min-device-pixel-ratio: 2) {\n            table {\n                min-width: auto !important;\n            }\n\n            .section_content_padded {\n                padding-right: 25px !important;\n                padding-left: 25px !important;\n            }\n\n            .counter {\n                font-size: 18px !important;\n            }\n        }\n    </style>\n</head>\n<body style=\"\tmargin: 0;\n\tbackground: #FEFEFE;\n\tcolor: #585858;\n\">\n    <!-- Preivew text -->\n    <span class=\"preheader\" style=\"display: none !important; visibility: hidden; opacity: 0; color: transparent; height: 0; width: 0;border-collapse: collapse;border: 0px;\"></span>\n    <!-- Carpool logo -->\n    <table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\">\n        <tbody>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <img src=\"https://media.licdn.com/dms/image/C4D0BAQFYfISsshjaNA/company-logo_200_200/0/1597744113257?e=2147483647&v=beta&t=mt3a8WUVVMk9isD7qn_DT_ssZfWlc8AIo7Re2Wux_PQ\" class=\"carpool_logo\" width=\"300\" height=\"300\" style=\"\tdisplay: block;\n\tmargin: 0 auto;\nmargin: 30px auto;border-radius:50%;object-fit:cover\">\n                </td>\n            </tr>\n            <!-- Header -->\n            <tr>\n                <td class=\"sectionlike imageless_section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n  background:  #1f9dff;\n  padding-bottom: 10px;\npadding-bottom: 20px;\"></td>\n            </tr>\n            <!-- Content -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  white;\n\">\n                        <tbody>\n                            <tr>\n                                <td class=\"section_content_padded\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 0 35px 40px;\">\n                                    <h1 style=\"\tfont-size: 20px;\n\tfont-weight: 500;\n\tmargin-top: 40px;\n\tmargin-bottom: 0;\n\">\n                                        Dəyərli HelloJob istifadəçisi, etdiyiniz müraciət qəbul edilib. Elan sahibi ilə əlaqə saxlamağı unutmayın.\n                                    </h1>\n                                    <p class=\"near_title last\" style=\"margin-top: 10px;margin-bottom: 0;\">Elan Yerləşdirdiyiniz üçün təşəkkürlər.</p>\n                                    <small style=\"\tdisplay: block;\n\twidth: 100%;\n\tmax-width: 330px;\n\tmargin: 14px auto 0;\n\tfont-size: 14px;\n\"></small>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Signature -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content section_zag\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  #1f9dff;\nbackground: #F4FBF9;\">\n                        <tbody>\n                            <tr>\n                                <td class=\"signature\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 20px;\">\n                                    <p class=\"marginless\" style=\"margin: 0;\"><br>HelloJob Team</p>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Footer -->\n            <tr>\n                <td class=\"section dummy_row\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\npadding-top: 20px !important;\"></td>\n            </tr>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n\tbackground: #fff;\n\">\n                    </table>\n                </td>\n            </tr>\n            <!-- Legal footer -->\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <p class=\"footer_legal\" style=\"\tpadding: 20px 0 40px;\n\tmargin: 0;\n\tfont-size: 12px;\n\tcolor: #A5A5A5;\n\tline-height: 1.5;\n\">\n                        Əgər bu mail sizə səhvən gəibsə zəhmət olmasa bizə bildirin<br><br>\n                        2023\n                        <br><br>\n\n                       HelloJob tərəfindən avtomatik göndırilmiş mail\n                    </p>\n                </td>\n            </tr>\n        </tbody>\n    </table>\n\n</body>", true);
        }
        public async Task DeleteCvRequestAsync(int requestId)
        {
            if (requestId <= 0) throw new WrongRequestException("The request sent does not exist");
            CvRequest item = await _repository.GetByIdCvRequest(requestId);
            if (item == null) throw new NotFoundException("Your request was not found");
            AppUser company = await _userManager.FindByIdAsync(item.AppUserId);
            _repository.DeleteCvRequest(item);
            await _repository.SaveChanceAsync();
            await _email.SendMailAsync(company.Email, "Cv-ə müraciətinin cavabı", "<head>\n    <meta charset=\"UTF-8\" />\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\n    <style type=\"text/css\">\n        body {\n            margin: 0;\n            background: #FEFEFE;\n            color: #585858;\n        }\n\n        table {\n            font-size: 15px;\n            line-height: 23px;\n            max-width: 500px;\n            min-width: 460px;\n            text-align: center;\n        }\n\n        .table_inner {\n            min-width: 100% !important;\n        }\n\n        td {\n            font-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n            vertical-align: top;\n        }\n\n        .carpool_logo {\n            margin: 30px auto;\n        }\n\n        .dummy_row {\n            padding-top: 20px !important;\n        }\n\n        .section,\n        .sectionlike {\n            background: #C9F9E9;\n        }\n\n        .section {\n            padding: 0 20px;\n        }\n\n        .sectionlike {\n            padding-bottom: 10px;\n        }\n\n        .section_content {\n            width: 100%;\n            background: #fff;\n        }\n\n        .section_content_padded {\n            padding: 0 35px 40px;\n        }\n\n        .section_zag {\n            background: #F4FBF9;\n        }\n\n        .imageless_section {\n            padding-bottom: 20px;\n        }\n\n        img {\n            display: block;\n            margin: 0 auto;\n        }\n\n        .img_section {\n            width: 100%;\n            max-width: 500px;\n        }\n\n        .img_section_side_table {\n            width: 100% !important;\n        }\n\n        h1 {\n            font-size: 20px;\n            font-weight: 500;\n            margin-top: 40px;\n            margin-bottom: 0;\n        }\n\n        .near_title {\n            margin-top: 10px;\n        }\n\n        .last {\n            margin-bottom: 0;\n        }\n\n        a {\n            color: #63D3CD;\n            font-weight: 500;\n            word-break: break-word; /* Footer has long unsubscribe link */\n        }\n\n        .button {\n            display: block;\n            width: 100%;\n            max-width: 300px;\n            background: #20DA9C;\n            border-radius: 8px;\n            color: #fff;\n            font-size: 18px;\n            font-weight: normal; /* Resetting from a */\n            padding: 12px 0;\n            margin: 30px auto 0;\n            text-decoration: none;\n        }\n\n        small {\n            display: block;\n            width: 100%;\n            max-width: 330px;\n            margin: 14px auto 0;\n            font-size: 14px;\n        }\n\n        .signature {\n            padding: 20px;\n        }\n\n        .footer,\n        .footer_like {\n            background: #1FD99A;\n        }\n\n        .footer {\n            padding: 0 20px 30px;\n        }\n\n        .footer_content {\n            width: 100%;\n            text-align: center;\n            font-size: 12px;\n            line-height: initial;\n            color: #005750;\n        }\n\n            .footer_content a {\n                color: #005750;\n            }\n\n        .footer_item_image {\n            margin: 0 auto 10px;\n        }\n\n        .footer_item_caption {\n            margin: 0 auto;\n        }\n\n        .footer_legal {\n            padding: 20px 0 40px;\n            margin: 0;\n            font-size: 12px;\n            color: #A5A5A5;\n            line-height: 1.5;\n        }\n\n        .text_left {\n            text-align: left;\n        }\n\n        .text_right {\n            text-align: right;\n        }\n\n        .va {\n            vertical-align: middle;\n        }\n\n        .stats {\n            min-width: auto !important;\n            max-width: 370px;\n            margin: 30px auto 0;\n        }\n\n        .counter {\n            font-size: 22px;\n        }\n\n        .stats_counter {\n            width: 23%;\n        }\n\n        .stats_image {\n            width: 18%;\n            padding: 0 10px;\n        }\n\n        .stats_meta {\n            width: 59%;\n        }\n\n        .stats_spaced {\n            padding-top: 16px;\n        }\n\n        .walkthrough_spaced {\n            padding-top: 24px;\n        }\n\n        .walkthrough {\n            max-width: none;\n        }\n\n        .walkthrough_meta {\n            padding-left: 20px;\n        }\n\n        .table_checkmark {\n            padding-top: 30px;\n        }\n\n        .table_checkmark_item {\n            font-size: 15px;\n        }\n\n        .td_checkmark {\n            width: 24px;\n            padding: 7px 12px 0 0;\n        }\n\n        .padded_bottom {\n            padding-bottom: 40px;\n        }\n\n        .marginless {\n            margin: 0;\n        }\n\n        /* Restricting responsive for iOS Mail app only as Inbox/Gmail have render bugs */\n        @media only screen and (max-width: 480px) and (-webkit-min-device-pixel-ratio: 2) {\n            table {\n                min-width: auto !important;\n            }\n\n            .section_content_padded {\n                padding-right: 25px !important;\n                padding-left: 25px !important;\n            }\n\n            .counter {\n                font-size: 18px !important;\n            }\n        }\n    </style>\n</head>\n<body style=\"\tmargin: 0;\n\tbackground: #FEFEFE;\n\tcolor: #585858;\n\">\n    <!-- Preivew text -->\n    <span class=\"preheader\" style=\"display: none !important; visibility: hidden; opacity: 0; color: transparent; height: 0; width: 0;border-collapse: collapse;border: 0px;\"></span>\n    <!-- Carpool logo -->\n    <table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\">\n        <tbody>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <img src=\"https://media.licdn.com/dms/image/C4D0BAQFYfISsshjaNA/company-logo_200_200/0/1597744113257?e=2147483647&v=beta&t=mt3a8WUVVMk9isD7qn_DT_ssZfWlc8AIo7Re2Wux_PQ\" class=\"carpool_logo\" width=\"300\" height=\"300\" style=\"\tdisplay: block;\n\tmargin: 0 auto;\nmargin: 30px auto;border-radius:50%;object-fit:cover\">\n                </td>\n            </tr>\n            <!-- Header -->\n            <tr>\n                <td class=\"sectionlike imageless_section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n  background:  #1f9dff;\n  padding-bottom: 10px;\npadding-bottom: 20px;\"></td>\n            </tr>\n            <!-- Content -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  white;\n\">\n                        <tbody>\n                            <tr>\n                                <td class=\"section_content_padded\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 0 35px 40px;\">\n                                    <h1 style=\"\tfont-size: 20px;\n\tfont-weight: 500;\n\tmargin-top: 40px;\n\tmargin-bottom: 0;\n\">\n                                        Dəyərli HelloJob istifadəçisi, etdiyiniz müraciət qəbul olunmayıb. Digər elanları gözdən keçirməyi unutmayın.\n                                    </h1>\n                                    <p class=\"near_title last\" style=\"margin-top: 10px;margin-bottom: 0;\">Elan Yerləşdirdiyiniz üçün təşəkkürlər.</p>\n                                    <small style=\"\tdisplay: block;\n\twidth: 100%;\n\tmax-width: 330px;\n\tmargin: 14px auto 0;\n\tfont-size: 14px;\n\"></small>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Signature -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content section_zag\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  #1f9dff;\nbackground: #F4FBF9;\">\n                        <tbody>\n                            <tr>\n                                <td class=\"signature\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 20px;\">\n                                    <p class=\"marginless\" style=\"margin: 0;\"><br>HelloJob Team</p>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Footer -->\n            <tr>\n                <td class=\"section dummy_row\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\npadding-top: 20px !important;\"></td>\n            </tr>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n\tbackground: #fff;\n\">\n                    </table>\n                </td>\n            </tr>\n            <!-- Legal footer -->\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <p class=\"footer_legal\" style=\"\tpadding: 20px 0 40px;\n\tmargin: 0;\n\tfont-size: 12px;\n\tcolor: #A5A5A5;\n\tline-height: 1.5;\n\">\n                        Əgər bu mail sizə səhvən gəibsə zəhmət olmasa bizə bildirin<br><br>\n                        2023\n                        <br><br>\n\n                       HelloJob tərəfindən avtomatik göndırilmiş mail\n                    </p>\n                </td>\n            </tr>\n        </tbody>\n    </table>\n\n</body>", true);
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

