using System;
namespace HelloJobFinal.Application.ViewModels
{
    public record UpdateSettingVm
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}

