using System;
namespace HelloJobFinal.Application.ViewModels
{
	public record ItemAppUserVm
	{
        public string Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
    }
}

