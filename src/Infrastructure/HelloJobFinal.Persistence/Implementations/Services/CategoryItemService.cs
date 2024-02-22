using System.Linq.Expressions;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class CategoryItemService : ICategoryItemService
	{
        private readonly IMapper _mapper;
        private readonly ICategoryItemRepository _repository;
        private readonly IBaseCategoryRepository _baseCategoryRepository;
        private readonly IHttpContextAccessor _http;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public CategoryItemService(UserManager<AppUser> userManager, IWebHostEnvironment env, IHttpContextAccessor http, IBaseCategoryRepository baseCategory, ICategoryItemRepository repository, IMapper mapper)
        {
            _userManager = userManager;
            _env = env;
            _http = http;
            _baseCategoryRepository = baseCategory;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(CreateCategoryItemVm create, ModelStateDictionary model)
        {
            if (!model.IsValid)
            {
                await CreatePopulateDropdowns(create);
                return false;
            }
            if (await _repository.CheckUniqueAsync(x => x.Name == create.Name))
            {
                model.AddModelError("Name", "Category item already exists.");
                return false;
            }
            if (!await _baseCategoryRepository.CheckUniqueAsync(x => x.Id == create.BaseCategoryId))
            {
                await CreatePopulateDropdowns(create);
                model.AddModelError("BaseCategoryId", "Base Category not found");
                return false;
            }
            CategoryItem item = _mapper.Map<CategoryItem>(create);

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(CategoryItem.Vacancies)}",
                $"{nameof(CategoryItem.Cvs)}",
                $"{nameof(CategoryItem.BaseCategory)}.{nameof(BaseCategory.CategoryItems)}" };
            CategoryItem item = await _repository.GetByIdAsync(id, includes: includes);
            if (item == null) throw new NotFoundException();

            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemCategoryItemVm>> GetAllWhereAsync(int take, int page = 1)
        {
            string[] includes = { $"{nameof(CategoryItem.Vacancies)}",
                $"{nameof(CategoryItem.Cvs)}",
                $"{nameof(CategoryItem.BaseCategory)}.{nameof(BaseCategory.CategoryItems)}" };

            ICollection<CategoryItem> items = await _repository
                    .GetAllWhere(skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCategoryItemVm> vMs = _mapper.Map<ICollection<ItemCategoryItemVm>>(items);

            return vMs;
        }

        public async Task<ICollection<ItemCategoryItemVm>> GetAllWhereByOrderAsync(int take, Expression<Func<CategoryItem, object>>? orderExpression, int page = 1)
        {
            string[] includes = { $"{nameof(CategoryItem.Vacancies)}",
                $"{nameof(CategoryItem.Cvs)}",
                $"{nameof(CategoryItem.BaseCategory)}"};

            ICollection<CategoryItem> items = await _repository
                    .GetAllWhereByOrder(orderException: orderExpression, skip: (page - 1) * take, take: take, IsTracking: false, includes: includes).ToListAsync();

            ICollection<ItemCategoryItemVm> vMs = _mapper.Map<ICollection<ItemCategoryItemVm>>(items);

            return vMs;
        }

        public async Task<GetCategoryItemVm> GetByIdAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            string[] includes = { $"{nameof(CategoryItem.Vacancies)}",
                $"{nameof(CategoryItem.Cvs)}",
                $"{nameof(CategoryItem.BaseCategory)}"};

            CategoryItem item = await _repository.GetByIdAsync(id, IsTracking: false, includes: includes);
            if (item == null) throw new NotFoundException();

            GetCategoryItemVm get = _mapper.Map<GetCategoryItemVm>(item);

            return get;
        }

        public async Task<PaginationVm<ItemCategoryItemVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(CategoryItem.Vacancies)}",
                $"{nameof(CategoryItem.Cvs)}",
                $"{nameof(CategoryItem.BaseCategory)}"};
            double count = await _repository.CountAsync();

            ICollection<CategoryItem> items = new List<CategoryItem>();

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

            ICollection<ItemCategoryItemVm> vMs = _mapper.Map<ICollection<ItemCategoryItemVm>>(items);

            PaginationVm<ItemCategoryItemVm> pagination = new PaginationVm<ItemCategoryItemVm>
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

        public async Task<PaginationVm<ItemCategoryItemVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException();
            if (order <= 0) throw new WrongRequestException();

            string[] includes = { $"{nameof(CategoryItem.Vacancies)}",
                $"{nameof(CategoryItem.Cvs)}",
                $"{nameof(CategoryItem.BaseCategory)}"};
            double count = await _repository.CountAsync();

            ICollection<CategoryItem> items = new List<CategoryItem>();

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

            ICollection<ItemCategoryItemVm> vMs = _mapper.Map<ICollection<ItemCategoryItemVm>>(items);

            PaginationVm<ItemCategoryItemVm> pagination = new PaginationVm<ItemCategoryItemVm>
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
            CategoryItem item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = false;
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            CategoryItem item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            item.IsDeleted = true;
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateCategoryItemVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            CategoryItem item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            UpdateCategoryItemVm update = _mapper.Map<UpdateCategoryItemVm>(item);
            await UpdatePopulateDropdowns(update);
            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateCategoryItemVm update, ModelStateDictionary model)
        {
            if (!model.IsValid)
            {
                await UpdatePopulateDropdowns(update);
                return false;
            }
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            CategoryItem item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException();

            if (await _repository.CheckUniqueAsync(x => x.Name.ToLower().Trim() == update.Name.ToLower().Trim() && x.Id != id))
            {
                model.AddModelError("Name", "Category item already exists.");
                return false;
            }
            if (!await _baseCategoryRepository.CheckUniqueAsync(x => x.Id == update.BaseCategoryId))
            {
                await UpdatePopulateDropdowns(update);
                model.AddModelError("BaseCategoryId", "Base Category not found");
                return false;
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateCategoryItemVm, CategoryItem>();
            });
            var mapper = config.CreateMapper();
            mapper.Map(update, item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task UpdatePopulateDropdowns(UpdateCategoryItemVm update)
        {
            update.Categorys = _mapper.Map<List<IncludeBaseCategoryVm>>(await _baseCategoryRepository.GetAll().ToListAsync());

        }
        public async Task CreatePopulateDropdowns(CreateCategoryItemVm create)
        {
            create.Categorys = _mapper.Map<List<IncludeBaseCategoryVm>>(await _baseCategoryRepository.GetAll().ToListAsync());
        }
    }
}

