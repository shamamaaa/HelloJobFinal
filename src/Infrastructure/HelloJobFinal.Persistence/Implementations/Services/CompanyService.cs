using System;
using System.Linq.Expressions;
using AutoMapper;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Application.ViewModels.Company;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Domain.Enums;
using HelloJobFinal.Infrastructure.Exceptions;
using HelloJobFinal.Infrastructure.Implementations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            item.ImageUrl = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
            
            //item.CreatedBy = _http.HttpContext.User.Identity.Name;

            await _repository.AddAsync(item);
            await _repository.SaveChanceAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            _repository.Delete(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<ICollection<ItemCompanyVm>> GetAllWhereAsync(int take, int page = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ItemCompanyVm>> GetAllWhereByOrderAsync(int take, Expression<Func<Company, object>>? orderExpression, int page = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<GetCompanyVm> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationVm<ItemCompanyVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationVm<ItemCompanyVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            throw new NotImplementedException();
        }

        public async Task ReverseSoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            item.IsDeleted = false;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            item.IsDeleted = true;
            _repository.Update(item);
            await _repository.SaveChanceAsync();
        }

        public async Task<UpdateCompanyVm> UpdateAsync(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");

            UpdateCompanyVm update = _mapper.Map<UpdateCompanyVm>(item);

            return update;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdateCompanyVm update, ModelStateDictionary model)
        {
            if (!model.IsValid) return false;
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Company item = await _repository.GetByIdAsync(id);
            if (item == null) throw new NotFoundException("Your request was not found");
            if (update.Photo != null)
            {
                if (!update.Photo.ValidateType())
                {
                    model.AddModelError("Photo", "File Not supported");
                    return false;
                }
                if (!update.Photo.ValidataSize())
                {
                    model.AddModelError("Photo", "Image should not be larger than 10 mb");
                    return false;
                }
                item.ImageUrl = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");

            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateCompanyVm, Company>()
                    .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
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

