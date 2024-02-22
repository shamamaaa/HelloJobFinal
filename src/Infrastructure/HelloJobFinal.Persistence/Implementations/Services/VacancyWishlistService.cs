using System;
using System.Security.Claims;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IVacancyRepository _VacancyRepository;
        private readonly UserManager<AppUser> _userManager;

        public VacancyWishlistService(UserManager<AppUser> userManager, IHttpContextAccessor http,
            IVacancyRepository VacancyRepository, IMapper mapper)
        {
            _userManager = userManager;
            _http = http;
            _VacancyRepository = VacancyRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<VacancyWishlistItemVm>> WishList()
        {
            ICollection<VacancyWishlistItemVm> wishLists = new List<VacancyWishlistItemVm>();
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(b => b.WishListVacancies).ThenInclude(p => p.Vacancy).ThenInclude(pi => pi.CategoryItem).ThenInclude(x=>x.BaseCategory)
                    .FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                foreach (var item in appUser.WishListVacancies)
                {
                    wishLists.Add(new VacancyWishlistItemVm
                    {
                        Id = item.Vacancy.Id,
                        Position = item.Vacancy.Position,
                        Salary = item.Vacancy.Salary,
                        CreatedAt = item.Vacancy.CreatedAt,
                        IncludeCategoryItem = _mapper.Map<IncludeCategoryItemVm>(item.Vacancy.CategoryItem)
                    });
                }
            }
            else
            {
                if (_http.HttpContext.Request.Cookies["HellojobVacancyWishlist"] is not null)
                {
                    ICollection<VacancyWishlistItemVm> wishes = JsonConvert.DeserializeObject<ICollection<VacancyWishlistItemVm>>(_http.HttpContext.Request.Cookies["HellojobVacancyWishlist"]);
                    foreach (VacancyWishlistItemVm wishListCookieItem in wishes)
                    {
                        Vacancy vacancy = await _VacancyRepository.GetByIdAsync(wishListCookieItem.Id, false, $"{nameof(Vacancy.CategoryItem)}");
                        if (vacancy is not null)
                        {
                            VacancyWishlistItemVm wish = new VacancyWishlistItemVm
                            {
                                Id = vacancy.Id,
                                Position = vacancy.Position,
                                Salary = vacancy.Salary,
                                CreatedAt = vacancy.CreatedAt,
                                IncludeCategoryItem = _mapper.Map<IncludeCategoryItemVm>(vacancy.CategoryItem)
                            };
                            wishLists.Add(wish);
                        }
                    }
                }
            }


            return wishLists;
        }

        public async Task AddWishList(int id)
        {
            if (id <= 0) throw new WrongRequestException();
            Vacancy Vacancy = await _VacancyRepository.GetByIdAsync(id);
            if (Vacancy == null) throw new NotFoundException();
            ICollection<VacancyWishlistCookieVM> cart;
            ICollection<IncludeVacancyVm> cartItems;

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(p => p.WishListVacancies).FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (appUser == null) throw new NotFoundException();
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
            if (id <= 0) throw new WrongRequestException();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(p => p.WishListVacancies).FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (appUser == null) throw new NotFoundException();

                WishListVacancy item = appUser.WishListVacancies.FirstOrDefault(b => b.VacancyId == id);

                if (item == null) throw new WrongRequestException();

                appUser.WishListVacancies.Remove(item);

                await _userManager.UpdateAsync(appUser);
            }
            else
            {
                ICollection<VacancyWishlistCookieVM> cart = JsonConvert.DeserializeObject<ICollection<VacancyWishlistCookieVM>>(_http.HttpContext.Request.Cookies["HellojobVacancyWishlist"]);

                VacancyWishlistCookieVM item = cart.FirstOrDefault(c => c.Id == id);

                if (item == null) throw new WrongRequestException();

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

