using System;
using System.Security.Claims;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Application.ViewModels;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class VacancyWishlistService : IVacancyWishlistService
    {
        private readonly IHttpContextAccessor _http;
        private readonly IVacancyRepository _VacancyRepository;
        private readonly UserManager<AppUser> _userManager;

        public VacancyWishlistService(UserManager<AppUser> userManager, IHttpContextAccessor http,
            IVacancyRepository VacancyRepository)
        {
            _userManager = userManager;
            _http = http;
            _VacancyRepository = VacancyRepository;
        }

        public async Task AddWishList(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Vacancy Vacancy = await _VacancyRepository.GetByIdAsync(id);
            if (Vacancy == null) throw new NotFoundException("Your request was not found");
            ICollection<VacancyWishlistCookieVM> cart;
            ICollection<IncludeVacancyVm> cartItems;

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(p => p.WishListVacancies).FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (appUser == null) throw new NotFoundException("Your request was not found");
                WishListVacancy item = appUser.WishListVacancies.FirstOrDefault(b => b.VacancyId == id);
                if (item == null)
                {
                    item = new WishListVacancy
                    {
                        AppUserId = appUser.Id,
                        VacancyId = Vacancy.Id,
                    };

                    appUser.WishListVacancies.Add(item);
                }
                await _userManager.UpdateAsync(appUser);
            }
            else
            {
                if (_http.HttpContext.Request.Cookies["HellojobVacancyWishlist"] is not null)
                {
                    cart = JsonConvert.DeserializeObject<ICollection<VacancyWishlistCookieVM>>(_http.HttpContext.Request.Cookies["HellojobVacancyWishlist"]);

                    VacancyWishlistCookieVM item = cart.FirstOrDefault(c => c.Id == id);
                    if (item == null)
                    {
                        VacancyWishlistCookieVM cartCookieItem = new VacancyWishlistCookieVM
                        {
                            Id = id
                        };
                        cart.Add(cartCookieItem);
                    }
                }
                else
                {
                    cart = new List<VacancyWishlistCookieVM>();
                    VacancyWishlistCookieVM cartCookieItem = new VacancyWishlistCookieVM
                    {
                        Id = id
                    };
                    cart.Add(cartCookieItem);
                }

                string json = JsonConvert.SerializeObject(cart);
                _http.HttpContext.Response.Cookies.Append("HellojobVacancyWishlist", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                });

            }
        }

        public async Task DeleteItem(int id)
        {
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(p => p.WishListVacancies).FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (appUser == null) throw new NotFoundException("Your request was not found");

                WishListVacancy item = appUser.WishListVacancies.FirstOrDefault(b => b.VacancyId == id);

                if (item == null) throw new WrongRequestException("The request sent does not exist");

                appUser.WishListVacancies.Remove(item);

                await _userManager.UpdateAsync(appUser);
            }
            else
            {
                ICollection<VacancyWishlistCookieVM> cart = JsonConvert.DeserializeObject<ICollection<VacancyWishlistCookieVM>>(_http.HttpContext.Request.Cookies["HellojobVacancyWishlist"]);

                VacancyWishlistCookieVM item = cart.FirstOrDefault(c => c.Id == id);

                if (item == null) throw new WrongRequestException("The request sent does not exist");

                cart.Remove(item);


                string json = JsonConvert.SerializeObject(cart);
                _http.HttpContext.Response.Cookies.Append("HellojobVacancyWishlist", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

            }
        }
    }

}

