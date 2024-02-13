using AutoMapper;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Domain.Enums;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class UserService : IUserService
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _http;
        private readonly IConfiguration _configuration;


        public UserService(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager,
            IEmailService emailService, IHttpContextAccessor http, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailService = emailService;
            _http = http;
            _configuration = configuration;
        }

        public async Task<PaginationVm<ItemAppUserVm>> GetFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            double count = await _userManager.Users.CountAsync();

            ICollection<AppUser> users = new List<AppUser>();

            switch (order)
            {
                case 1:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderBy(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 2:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderByDescending(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 3:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == false).OrderBy(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 4:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                    .Where(x => x.IsActivate == false).OrderByDescending(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
            }

            ICollection<ItemAppUserVm> vMs = _mapper.Map<ICollection<ItemAppUserVm>>(users);

            PaginationVm<ItemAppUserVm> pagination = new PaginationVm<ItemAppUserVm>
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

        public async Task<PaginationVm<ItemAppUserVm>> GetDeleteFilteredAsync(string? search, int take, int page, int order)
        {
            if (page <= 0) throw new WrongRequestException("The request sent does not exist");
            if (order <= 0) throw new WrongRequestException("The request sent does not exist");

            double count = await _userManager.Users.CountAsync();

            ICollection<AppUser> users = new List<AppUser>();

            switch (order)
            {
                case 1:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderBy(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 2:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderByDescending(x => x.UserName).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 3:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                        .Where(x => x.IsActivate == true).OrderBy(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
                case 4:
                    users = await _userManager.Users.Where(x => !string.IsNullOrEmpty(search) ? x.UserName.ToLower().Contains(search.ToLower()) : true)
                        .Where(x => x.UserName != _configuration["AdminSettings:UserName"] && x.UserName != _configuration["ModeratorSettings:UserName"])
                    .Where(x => x.IsActivate == true).OrderByDescending(x => x.Name).Skip((page - 1) * take).Take(take).AsNoTracking().ToListAsync();
                    break;
            }

            ICollection<ItemAppUserVm> vMs = _mapper.Map<ICollection<ItemAppUserVm>>(users);

            PaginationVm<ItemAppUserVm> pagination = new PaginationVm<ItemAppUserVm>
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

        public async Task<GetAppUserVM> GetByIdAdminAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.Users
                .Include(x => x.WishListCvs).ThenInclude(x => x.Cv)
                .Include(x => x.WishListVacancies).ThenInclude(x => x.Vacancy)
                .Include(x => x.Companies).Include(x => x.Cvs)
                .Include(x => x.CvRequests).ThenInclude(x => x.Cv)
                .Include(x => x.VacancyRequests).ThenInclude(x => x.Vacancy)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) throw new NotFoundException("Your request was not found");

            GetAppUserVM get = _mapper.Map<GetAppUserVM>(user);

            return get;
        }

        public async Task<GetAppUserVM> GetByUserNameAdminAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.Users
                .Include(x => x.WishListCvs).ThenInclude(x => x.Cv)
                .Include(x => x.WishListVacancies).ThenInclude(x => x.Vacancy)
                .Include(x => x.Companies).Include(x => x.Cvs)
                .Include(x => x.CvRequests).ThenInclude(x => x.Cv)
                .Include(x => x.VacancyRequests).ThenInclude(x => x.Vacancy)
                .AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null) throw new NotFoundException("Your request was not found");

            GetAppUserVM get = _mapper.Map<GetAppUserVM>(user);

            return get;
        }

        public async Task<GetAppUserVM> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.Users
                .Include(x => x.WishListCvs).ThenInclude(x => x.Cv)
                .Include(x => x.WishListVacancies).ThenInclude(x => x.Vacancy)
                .Include(x => x.Companies).Include(x => x.Cvs)
                .Include(x => x.CvRequests).ThenInclude(x => x.Cv)
                .Include(x => x.VacancyRequests).ThenInclude(x => x.Vacancy)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) throw new NotFoundException("Your request was not found");

            GetAppUserVM get = _mapper.Map<GetAppUserVM>(user);

            return get;
        }

        public async Task<GetAppUserVM> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.Users
                .Include(x => x.WishListCvs).ThenInclude(x => x.Cv)
                .Include(x => x.WishListVacancies).ThenInclude(x => x.Vacancy)
                .Include(x => x.Companies).Include(x => x.Cvs)
                .Include(x => x.CvRequests).ThenInclude(x => x.Cv)
                .Include(x => x.VacancyRequests).ThenInclude(x => x.Vacancy)
                .AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null) throw new NotFoundException("Your request was not found");

            GetAppUserVM get = _mapper.Map<GetAppUserVM>(user);

            return get;
        }

        public async Task GiveRoleModeratorAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            await _userManager.AddToRoleAsync(user, AdminRole.Moderator.ToString());
        }
        public async Task DeleteRoleModeratorAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            await _userManager.AddToRoleAsync(user, AdminRole.Moderator.ToString());
        }
        public async Task ReverseSoftDeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            user.IsActivate = false;

            await _userManager.UpdateAsync(user);
        }

        public async Task SoftDeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            user.IsActivate = true;

            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            await _userManager.DeleteAsync(user);
        }

        public async Task<EditAppUserVm> EditUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.Users
                .Include(x => x.Products).ThenInclude(x => x.Category)
                .Include(x => x.Products).ThenInclude(x => x.ProductImages).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) throw new NotFoundException("Your request was not found");

            EditAppUserVm get = _mapper.Map<EditAppUserVm>(user);

            return get;
        }

        public async Task<bool> EditUserAsync(string id, EditAppUserVm update, ModelStateDictionary model)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            _mapper.Map(update, user);
            user.Name = user.Name.Capitalize();
            user.Surname = user.Surname.Capitalize();
            if (update.Photo != null)
            {
                await 
                user.Img = await _cLoud.FileCreateAsync(update.Photo);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);

            return true;
        }

        public async Task ForgotPassword(string id, IUrlHelper url)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = url.Action("ChangePassword", "User", new { Id = user.Id, Token = token }, _http.HttpContext.Request.Scheme);
            await _emailService.SendMailAsync(user.Email, "Password Reset", confirmationLink);
        }

        public async Task<bool> ChangePassword(string id, string token, ForgotPasswordVm fogotPassword, ModelStateDictionary model)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(token)) throw new NotFoundException("Your request was not found");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");

            var result = await _userManager.ChangePasswordAsync(user, fogotPassword.Password, fogotPassword.NewPassword);
            if (!result.Succeeded)
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description;
                }
                throw new WrongRequestException(errors);
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);
            return true;
        }
    }
}

