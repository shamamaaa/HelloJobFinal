using System;
namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface IVacancyWishlistService
	{
        Task AddWishList(int id);
        Task DeleteItem(int id);
    }
}

