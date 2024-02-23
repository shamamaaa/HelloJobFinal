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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _repository;
        private readonly IHttpContextAccessor _http;
        private readonly IWebHostEnvironment _env;

        public CompanyService(IMapper mapper, ICompanyRepository repository,
            IHttpContextAccessor http, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _repository = repository;
            _http = http;
            _env = env;
        }

        public async Task<bool> CreateAsync(CreateCompanyVm create, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;

            if (!create.Photo.ValidateType())
            {
                model.AddModelError("Photo", "File Not supported");
                return false;
            }
            if (!create.Photo.ValidataSize())
            {
                model.AddModelError("Photo", "Image should not be larger than 10 mb");
                return false;
            }

            Company item = _mapper.Map<Company>(create);

            item.Status = Status.New.ToString();
            item.ImageUrl = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images","Company");
            
            item.CreatedBy = _http.HttpContext.User.Identity.Name;
            item.AppUserId = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "images", "Company");

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemCompanyVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes = { $"{nameof(Company.Vacancies)}" };

            ICollection<Company> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCompanyVm> vMs = _mapper.Map<ICollection<ItemCompanyVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemCompanyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Company, object>>? orderExpression, int page = 1)
        {
            string[] includes = { $"{nameof(Company.Vacancies)}" };

            ICollection<Company> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCompanyVm> vMs = _mapper.Map<ICollection<ItemCompanyVm>>(items);

            return vMs;
        }

        public async Task<GetCompanyVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(Company.Vacancies)}.{nameof(Vacancy.WishListVacancies)}",
            $"{nameof(Company.Vacancies)}.{nameof(Vacancy.CategoryItem)}.{nameof(CategoryItem.BaseCategory)}"};

            Company item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException();

            GetCompanyVm get = _mapper.Map<GetCompanyVm>(item);
            foreach (var vacancy in get.Vacancies)
            {
                vacancy.VacancyIds = item.Vacancies
                                    .SelectMany(x => x.WishListVacancies)
                                    .Select(a => a.VacancyId)
                                    .ToList();
            }

            return get;
        }

        public async Task<PaginationVm<ItemCompanyVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(Company.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<Company> items = new List<Company>();

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

            ICollection<ItemCompanyVm> vMs = _mapper.Map<ICollection<ItemCompanyVm>>(items);

            PaginationVm<ItemCompanyVm> pagination = new PaginationVm<ItemCompanyVm>
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

        public async Task<PaginationVm<ItemCompanyVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(Company.Vacancies)}" };
            double count = await _repository.CountAsync();

            ICollection<Company> items = new List<Company>();

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

            ICollection<ItemCompanyVm> vMs = _mapper.Map<ICollection<ItemCompanyVm>>(items);

            PaginationVm<ItemCompanyVm> pagination = new PaginationVm<ItemCompanyVm>
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
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();
            item.IsDeleted = false;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();
            item.IsDeleted = true;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateCompanyVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            UpdateCompanyVm update = _mapper.Map<UpdateCompanyVm>(item);

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateCompanyVm update, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower().Trim() == update.Name.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Name", "Company already exists.");
                return false;
            }
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(Company.Vacancies)}" };

            Company item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            if (update.Photo != null)
            {
                if (!update.Photo.ValidateType())
                {
                    model.AddModelError("Photo", "File type is not valid, please choose image file.");
                    return false;
                }
                if (!update.Photo.ValidataSize())
                {
                    model.AddModelError("Photo", "File size is not valid, please choose less than 5Mb.");
                    return false;
                }

                item.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "images", "Company");
                item.ImageUrl = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "Company");
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateCompanyVm, Company>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            await _repository.SaveChanceAsync();

            return true;
        }
    }
}

