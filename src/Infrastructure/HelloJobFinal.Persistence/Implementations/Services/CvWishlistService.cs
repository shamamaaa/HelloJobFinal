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


namespace HelloJobFinal.Persistence.Implementations.Services
{
    public class CvWishlistService : ICvWishlistService
    {
        private readonly IHttpContextAccessor _http;
        private readonly ICvRepository _cvRepository;
        private readonly UserManager<AppUser> _userManager;

        public CvWishlistService(UserManager<AppUser> userManager, IHttpContextAccessor http,
            ICvRepository cvRepository)
        {
            _userManager = userManager;
            _http = http;
            _cvRepository = cvRepository;
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

