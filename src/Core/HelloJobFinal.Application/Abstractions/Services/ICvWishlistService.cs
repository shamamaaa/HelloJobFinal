using System;
using HelloJobFinal.Application.ViewModels;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface ICvWishlistService
    {
        Task<ICollection<CvWishlistItemVm>> WishList();
        Task AddWishList(int id);
        Task DeleteItem(int id);
    }
}

