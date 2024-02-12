using System;
namespace HelloJobFinal.Application.Abstractions.Services
{
	public interface ICvWishlistService
	{
        Task AddWishList(int id);
        Task DeleteItem(int id);
    }
}

