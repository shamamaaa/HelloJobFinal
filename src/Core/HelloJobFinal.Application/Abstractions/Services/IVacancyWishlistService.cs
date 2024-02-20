using System;
using HelloJobFinal.Application.ViewModels;

namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IVacancyWishlistService
	{
        Task<ICollection<VacancyWishlistItemVm>> WishList();
        Task AddWishList(int id);
        Task DeleteItem(int id);
    }
}

