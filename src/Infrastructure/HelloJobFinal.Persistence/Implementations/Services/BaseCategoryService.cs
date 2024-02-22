using System.Linq.Expressions;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
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
    public class BaseCategoryService : IBaseCategoryService
	{
        private readonly IMapper _mapper;
        private readonly IBaseCategoryRepository _repository;
        private readonly IHttpContextAccessor _http;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public BaseCategoryService(IMapper mapper, IBaseCategoryRepository repository,
            IHttpContextAccessor http, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _repository = repository;
            _http = http;
            _userManager = userManager;
            _env = env;
        }

        public async Task<bool> CreateAsync(CreateBaseCategoryVm create, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name == create.Name))
            {
                model.AddModelError("Name", "Base category already exists.");
                return false;
            }
            if (!create.Photo.ValidateType())
            {
                model.AddModelError("Photo", "File type is not valid, please choose image file.");
                return false;
            }
            if (!create.Photo.ValidataSize())
            {
                model.AddModelError("Photo", "File size is not valid, please choose less than 5Mb.");
                return false;
            }

            BaseCategory item = _mapper.Map<BaseCategory>(create);

            item.ImageUrl = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "BusinessTitle");

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(BaseCategory.CategoryItems)}" };
            BaseCategory item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            item.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "images", "BusinessTitle");

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemBaseCategoryVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes = {$"{nameof(BaseCategory.CategoryItems)}.{nameof(CategoryItem.Vacancies)}" };
            ICollection<BaseCategory> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemBaseCategoryVm> vMs = _mapper.Map<ICollection<ItemBaseCategoryVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemBaseCategoryVm>> GetAllWhereByOrderAsync(int take, Expression<Func<BaseCategory, object>>? orderExpression, int page = 1)
        {
            string[] includes = { $"{nameof(BaseCategory.CategoryItems)}" };

            ICollection<BaseCategory> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemBaseCategoryVm> vMs = _mapper.Map<ICollection<ItemBaseCategoryVm>>(items);

            return vMs;
        }

        public async Task<GetBaseCategoryVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(BaseCategory.CategoryItems)}" };

            BaseCategory item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException();

            GetBaseCategoryVm get = _mapper.Map<GetBaseCategoryVm>(item);

            return get;
        }

        public async Task<PaginationVm<ItemBaseCategoryVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(BaseCategory.CategoryItems)}" };
            double count = await _repository.CountAsync();

            ICollection<BaseCategory> items = new List<BaseCategory>();

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

            ICollection<ItemBaseCategoryVm> vMs = _mapper.Map<ICollection<ItemBaseCategoryVm>>(items);

            PaginationVm<ItemBaseCategoryVm> pagination = new PaginationVm<ItemBaseCategoryVm>
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

        public async Task<PaginationVm<ItemBaseCategoryVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(BaseCategory.CategoryItems)}" };
            double count = await _repository.CountAsync();

            ICollection<BaseCategory> items = new List<BaseCategory>();

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

            ICollection<ItemBaseCategoryVm> vMs = _mapper.Map<ICollection<ItemBaseCategoryVm>>(items);

            PaginationVm<ItemBaseCategoryVm> pagination = new PaginationVm<ItemBaseCategoryVm>
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
            BaseCategory item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = false;
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            BaseCategory item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = true;
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateBaseCategoryVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            BaseCategory item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            UpdateBaseCategoryVm update = _mapper.Map<UpdateBaseCategoryVm>(item);

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateBaseCategoryVm update, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower().Trim() == update.Name.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Name", "Base category already exists.");
                return false;
            }
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(BaseCategory.CategoryItems)}" };

            BaseCategory item = await _repository.GetByIdAsync(id, includes: includes);
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

                item.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "images", "BusinessTitle");
                item.ImageUrl = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "BusinessTitle");
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateBaseCategoryVm, BaseCategory>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            });
            var mapper = config.CreateMapper();

            mapper.Map(update, item);
            await _repository.SaveChanceAsync();

            return true;
        }
    }
}

