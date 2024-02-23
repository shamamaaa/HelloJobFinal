using System;
namespace HelloJobFinal.Application.ViewModels
{
	public record IncludeCvRequestVm
	{
        public IncludeAppUserVm IncludeAppUser { get; init; }
        public string AppUserId { get; init; }
    }
}

