using System;
using System.Linq.Expressions;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Category;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelloJobFinal.Persistence.Implementations.Services
{
	public class BaseCategoryService : IBaseCategoryService
	{
        private readonly IMapper _mapper;
        private readonly IBaseCategoryRepository _repository;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<AppUser> _userManager;

        public BaseCategoryService(IMapper mapper, IBaseCategoryRepository repository,
            IHttpContextAccessor http, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _http = http;
            _userManager = userManager;
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

            item.ImageUrl = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images","");

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ItemBaseCategoryVm>> GetAllWhereAsync(int take, int page = 1)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ItemBaseCategoryVm>> GetAllWhereByOrderAsync(int take, Expression<Func<BaseCategory, object>>? orderExpression, int page = 1)
        {
            throw new NotImplementedException();
        }

        public Task<GetBaseCategoryVm> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationVm<ItemBaseCategoryVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationVm<ItemBaseCategoryVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            throw new NotImplementedException();
        }

        public Task ReverseSoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateBaseCategoryVm> UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePostAsync(int id, UpdateBaseCategoryVm update, ModelStateDictionary model)
        {
            throw new NotImplementedException();
        }
    }
}

