using System.Security.Claims;
using HelloJobFinal.Application.Abstractions.Repositories;
using HelloJobFinal.Application.Abstractions.Services;
using HelloJobFinal.Domain.Entities;
using HelloJobFinal.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HelloJobFinal.Application.ViewModels;
using AutoMapper;

namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class CvWishlistService : ICvWishlistService
    {
        private readonly IHttpContextAccessor _http;
        private readonly IMapper _mapper;
        private readonly ICvRepository _cvRepository;
        private readonly UserManager<AppUser> _userManager;

        public CvWishlistService(UserManager<AppUser> userManager, IHttpContextAccessor http,
            ICvRepository cvRepository, IMapper mapper)
        {
            _userManager = userManager;
            _http = http;
            _cvRepository = cvRepository;
            _mapper = mapper;
        }
        public async Task<ICollection<CvWishlistItemVm>> WishList()
        {
            ICollection<CvWishlistItemVm> wishLists = new List<CvWishlistItemVm>();
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(b => b.WishListCvs).ThenInclude(p => p.Cv).ThenInclude(pi => pi.WorkingHour)
                    .Include(b => b.WishListCvs).ThenInclude(p => p.Cv).ThenInclude(pi => pi.Experience)
                    .FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                foreach (var item in appUser.WishListCvs)
                {
                    wishLists.Add(new CvWishlistItemVm
                    {
                        Id = item.Cv.Id,
                        Position = item.Cv.Position,
                        Salary = item.Cv.MinSalary,
                        Name = item.Cv.Name,
                        Surname = item.Cv.Surname,
                        IncludeExperienceVm = _mapper.Map<IncludeExperienceVm>(item.Cv.Experience),
                        IncludWorkingHourVm = _mapper.Map<IncludWorkingHourVm>(item.Cv.WorkingHour)
                    });
                }
            }
            else
            {
                if (_http.HttpContext.Request.Cookies["HellojobCvWishlist"] is not null)
                {
                    ICollection<CvWishlistItemVm> wishes = JsonConvert.DeserializeObject<ICollection<CvWishlistItemVm>>(_http.HttpContext.Request.Cookies["HellojobCvWishlist"]);
                    foreach (CvWishlistItemVm wishListCookieItem in wishes)
                    {
                        Cv cv = await _cvRepository.GetByIdAsync(wishListCookieItem.Id, false, $"{nameof(Cv.Experience)}", $"{nameof(Cv.WorkingHour)}");
                        if (cv is not null)
                        {
                            CvWishlistItemVm wish = new CvWishlistItemVm
                            {
                                Id = cv.Id,
                                Position = cv.Position,
                                Salary = cv.MinSalary,
                                Name = cv.Name,
                                Surname = cv.Surname,
                                IncludeExperienceVm = _mapper.Map<IncludeExperienceVm>(cv.Experience),
                                IncludWorkingHourVm = _mapper.Map<IncludWorkingHourVm>(cv.WorkingHour)
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
            if (id <= 0) throw new WrongRequestException("The request sent does not exist");
            Cv Cv = await _cvRepository.GetByIdAsync(id);
            if (Cv == null) throw new NotFoundException("Your request was not found");
            ICollection<CvWishlistCookieVM> cart;
            ICollection<IncludeCvVm> cartItems;

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(p => p.WishListCvs).FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (appUser == null) throw new NotFoundException("Your request was not found");
                WishListCv item = appUser.WishListCvs.FirstOrDefault(b => b.CvId == id);
                if (item == null)
                {
                    item = new WishListCv
                    {
                        AppUserId = appUser.Id,
                        CvId = Cv.Id,
                    };

                    appUser.WishListCvs.Add(item);
                }
                await _userManager.UpdateAsync(appUser);
            }
            else
            {
                if (_http.HttpContext.Request.Cookies["HellojobCvWishlist"] is not null)
                {
                    cart = JsonConvert.DeserializeObject<ICollection<CvWishlistCookieVM>>(_http.HttpContext.Request.Cookies["HellojobCvWishlist"]);

                    CvWishlistCookieVM item = cart.FirstOrDefault(c => c.Id == id);
                    if (item == null)
                    {
                        CvWishlistCookieVM cartCookieItem = new CvWishlistCookieVM
                        {
                            Id = id
                        };
                        cart.Add(cartCookieItem);
                    }
                }
                else
                {
                    cart = new List<CvWishlistCookieVM>();
                    CvWishlistCookieVM cartCookieItem = new CvWishlistCookieVM
                    {
                        Id = id
                    };
                    cart.Add(cartCookieItem);
                }

                string json = JsonConvert.SerializeObject(cart);
                _http.HttpContext.Response.Cookies.Append("HellojobCvWishlist", json, new CookieOptions
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
                    .Include(p => p.WishListCvs).FirstOrDefaultAsync(u => u.Id == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (appUser == null) throw new NotFoundException("Your request was not found");

                WishListCv item = appUser.WishListCvs.FirstOrDefault(b => b.CvId == id);

                if (item == null) throw new WrongRequestException("The request sent does not exist");

                appUser.WishListCvs.Remove(item);

                await _userManager.UpdateAsync(appUser);
            }
            else
            {
                ICollection<CvWishlistCookieVM> cart = JsonConvert.DeserializeObject<ICollection<CvWishlistCookieVM>>(_http.HttpContext.Request.Cookies["HellojobCvWishlist"]);

                CvWishlistCookieVM item = cart.FirstOrDefault(c => c.Id == id);

                if (item == null) throw new WrongRequestException("The request sent does not exist");

                cart.Remove(item);


                string json = JsonConvert.SerializeObject(cart);
                _http.HttpContext.Response.Cookies.Append("HellojobCvWishlist", json, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

            }
        }
    }

}

