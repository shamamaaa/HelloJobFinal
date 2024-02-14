using AutoMapper;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Domain.Enums;
using HelloJobFinal.Infrastructure.Exceptions;
using HelloJobFinal.Infrastructure.Implementations;
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

        //public async Task<EditAppUserVm> EditUser(string id)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
        //    AppUser user = await _userManager.Users
        //        .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        //    if (user == null) throw new NotFoundException("Your request was not found");

        //    EditAppUserVm get = _mapper.Map<EditAppUserVm>(user);

        //    return get;
        //}

        //public async Task<bool> EditUserAsync(string id, EditAppUserVm update, ModelStateDictionary model)
        //{
        //    if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
        //    AppUser user = await _userManager.FindByIdAsync(id);
        //    if (user == null) throw new NotFoundException("Your request was not found");

        //    _mapper.Map(update, user);
        //    user.Name = user.Name.Capitalize();
        //    user.Surname = user.Surname.Capitalize();
        //    if (update.Photo != null)
        //    {
        //        await 
        //        user.im = await _cLoud.FileCreateAsync(update.Photo);
        //    }

        //    await _userManager.UpdateAsync(user);
        //    await _signInManager.SignOutAsync();
        //    await _signInManager.SignInAsync(user, false);

        //    return true;
        //}

        public async Task ForgotPassword(string id, IUrlHelper url)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new WrongRequestException("The request sent does not exist");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("Your request was not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = url.Action("ChangePassword", "User", new { Id = user.Id, Token = token }, _http.HttpContext.Request.Scheme);
            await _emailService.SendMailAsync(user.Email, "Password Reset", $"<head>\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <style type=\"text/css\">\n        body {{\n            margin: 0;\n            background: #FEFEFE;\n            color: #585858;\n        }}\n\n        table {{\n            font-size: 15px;\n            line-height: 23px;\n            max-width: 500px;\n            min-width: 460px;\n            text-align: center;\n        }}\n\n        .table_inner {{\n            min-width: 100% !important;\n        }}\n\n        td {{\n            font-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n            vertical-align: top;\n        }}\n\n        .carpool_logo {{\n            margin: 30px auto;\n        }}\n\n        .dummy_row {{\n            padding-top: 20px !important;\n        }}\n\n        .section,\n        .sectionlike {{\n            background: #C9F9E9;\n        }}\n\n        .section {{\n            padding: 0 20px;\n        }}\n\n        .sectionlike {{\n            padding-bottom: 10px;\n        }}\n\n        .section_content {{\n            width: 100%;\n            background: #fff;\n        }}\n\n        .section_content_padded {{\n            padding: 0 35px 40px;\n        }}\n\n        .section_zag {{\n            background: #F4FBF9;\n        }}\n\n        .imageless_section {{\n            padding-bottom: 20px;\n        }}\n\n        img {{\n            display: block;\n            margin: 0 auto;\n        }}\n\n        .img_section {{\n            width: 100%;\n            max-width: 500px;\n        }}\n\n        .img_section_side_table {{\n            width: 100% !important;\n        }}\n\n        h1 {{\n            font-size: 20px;\n            font-weight: 500;\n            margin-top: 40px;\n            margin-bottom: 0;\n        }}\n\n        .near_title {{\n            margin-top: 10px;\n        }}\n\n        .last {{\n            margin-bottom: 0;\n        }}\n\n        a {{\n            color: #63D3CD;\n            font-weight: 500;\n            word-break: break-word; /* Footer has long unsubscribe link */\n        }}\n\n        .button {{\n            display: block;\n            width: 100%;\n            max-width: 300px;\n            background: #20DA9C;\n            border-radius: 8px;\n            color: #fff;\n            font-size: 18px;\n            font-weight: normal; /* Resetting from a */\n            padding: 12px 0;\n            margin: 30px auto 0;\n            text-decoration: none;\n        }}\n\n        small {{\n            display: block;\n            width: 100%;\n            max-width: 330px;\n            margin: 14px auto 0;\n            font-size: 14px;\n        }}\n\n        .signature {{\n            padding: 20px;\n        }}\n\n        .footer,\n        .footer_like {{\n            background: #1FD99A;\n        }}\n\n        .footer {{\n            padding: 0 20px 30px;\n        }}\n\n        .footer_content {{\n            width: 100%;\n            text-align: center;\n            font-size: 12px;\n            line-height: initial;\n            color: #005750;\n        }}\n\n            .footer_content a {{\n                color: #005750;\n            }}\n\n        .footer_item_image {{\n            margin: 0 auto 10px;\n        }}\n\n        .footer_item_caption {{\n            margin: 0 auto;\n        }}\n\n        .footer_legal {{\n            padding: 20px 0 40px;\n            margin: 0;\n            font-size: 12px;\n            color: #A5A5A5;\n            line-height: 1.5;\n        }}\n\n        .text_left {{\n            text-align: left;\n        }}\n\n        .text_right {{\n            text-align: right;\n        }}\n\n        .va {{\n            vertical-align: middle;\n        }}\n\n        .stats {{\n            min-width: auto !important;\n            max-width: 370px;\n            margin: 30px auto 0;\n        }}\n\n        .counter {{\n            font-size: 22px;\n        }}\n\n        .stats_counter {{\n            width: 23%;\n        }}\n\n        .stats_image {{\n            width: 18%;\n            padding: 0 10px;\n        }}\n\n        .stats_meta {{\n            width: 59%;\n        }}\n\n        .stats_spaced {{\n            padding-top: 16px;\n        }}\n\n        .walkthrough_spaced {{\n            padding-top: 24px;\n        }}\n\n        .walkthrough {{\n            max-width: none;\n        }}\n\n        .walkthrough_meta {{\n            padding-left: 20px;\n        }}\n\n        .table_checkmark {{\n            padding-top: 30px;\n        }}\n\n        .table_checkmark_item {{\n            font-size: 15px;\n        }}\n\n        .td_checkmark {{\n            width: 24px;\n            padding: 7px 12px 0 0;\n        }}\n\n        .padded_bottom {{\n            padding-bottom: 40px;\n        }}\n\n        .marginless {{\n            margin: 0;\n        }}\n\n        /* Restricting responsive for iOS Mail app only as Inbox/Gmail have render bugs */\n        @media only screen and (max-width: 480px) and (-webkit-min-device-pixel-ratio: 2) {{\n            table {{\n                min-width: auto !important;\n            }}\n\n            .section_content_padded {{\n                padding-right: 25px !important;\n                padding-left: 25px !important;\n            }}\n\n            .counter {{\n                font-size: 18px !important;\n            }}\n        }}\n    </style>\n</head>\n<body style=\"\tmargin: 0;\n\tbackground: #FEFEFE;\n\tcolor: #585858;\n\">\n    <!-- Preivew text -->\n    <span class=\"preheader\" style=\"display: none !important; visibility: hidden; opacity: 0; color: transparent; height: 0; width: 0;border-collapse: collapse;border: 0px;\"></span>\n    <!-- Carpool logo -->\n    <table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\">\n        <tbody>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <img src=\"https://media.licdn.com/dms/image/C4D0BAQFYfISsshjaNA/company-logo_200_200/0/1597744113257?e=2147483647&v=beta&t=mt3a8WUVVMk9isD7qn_DT_ssZfWlc8AIo7Re2Wux_PQ\" class=\"carpool_logo\" width=\"300\" height=\"300\" style=\"\tdisplay: block;\n\tmargin: 0 auto;\nmargin: 30px auto;border-radius:50%;object-fit:cover\">\n                </td>\n            </tr>\n            <!-- Header -->\n            <tr>\n                <td class=\"sectionlike imageless_section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n  background:  #1f9dff;\n  padding-bottom: 10px;\npadding-bottom: 20px;\"></td>\n            </tr>\n            <!-- Content -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  white;\n\">\n                        <tbody>\n                            <tr>\n                                <td class=\"section_content_padded\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 0 35px 40px;\">\n                                    <h1 style=\"\tfont-size: 20px;\n\tfont-weight: 500;\n\tmargin-top: 40px;\n\tmargin-bottom: 0;\n\">\n\n\n                                        Dear {user.Name}{user.Surname},  As HelloJOb Team, we have sent a link to change your password.\n                                    </h1>\n                                    <p class=\"near_title last\" style=\"margin-top: 10px;margin-bottom: 0;\">Please click the button.</p>\n                                    <a href=\"{confirmationLink}\" style=\"\tdisplay: block;\n\twidth: 100%;\n\tmax-width: 300px;\n\tbackground:  #061e40 ;\n\tborder-radius: 8px;\n\tcolor: #fff;\n\tfont-size: 18px;\n\tpadding: 12px 0;\n\tmargin: 30px auto 0;\n\ttext-decoration: none;\n\" target=\"_blank\">Reset Password</a>\n                                    <small style=\"\tdisplay: block;\n\twidth: 100%;\n\tmax-width: 330px;\n\tmargin: 14px auto 0;\n\tfont-size: 14px;\n\"></small>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Signature -->\n            <tr>\n                <td class=\"section\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content section_zag\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n    background:  #1f9dff;\nbackground: #F4FBF9;\">\n                        <tbody>\n                            <tr>\n                                <td class=\"signature\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\npadding: 20px;\">\n                                    <p class=\"marginless\" style=\"margin: 0;\"><br>HelloJob Team</p>\n                                </td>\n                            </tr>\n                        </tbody>\n                    </table>\n                </td>\n            </tr>\n            <!-- Footer -->\n            <tr>\n                <td class=\"section dummy_row\" style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n    background:  #1f9dff;\n\tpadding: 0 20px;\npadding-top: 20px !important;\"></td>\n            </tr>\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"section_content\" style=\"\tfont-size: 15px;\n\tline-height: 23px;\n\tmax-width: 500px;\n\tmin-width: 460px;\n\ttext-align: center;\n\twidth: 100%;\n\tbackground: #fff;\n\">\n                    </table>\n                </td>\n            </tr>\n            <!-- Legal footer -->\n            <tr>\n                <td style=\"\tfont-family: -apple-system, BlinkMacSystemFont, Roboto, sans-serif;\n\tvertical-align: top;\n    border: none !important;\n\">\n                    <p class=\"footer_legal\" style=\"\tpadding: 20px 0 40px;\n\tmargin: 0;\n\tfont-size: 12px;\n\tcolor: #A5A5A5;\n\tline-height: 1.5;\n\">\n                        If you did not enter this email address when signing up for HelloJob service, disregard this message.<br><br>\n                        2023\n                        <br><br>\n\n                        This is a mandatory service email from HelloJob.\n                    </p>\n                </td>\n            </tr>\n        </tbody>\n    </table>\n\n</body>",true);
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

