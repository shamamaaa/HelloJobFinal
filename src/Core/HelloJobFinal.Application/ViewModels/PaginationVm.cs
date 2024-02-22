using System;
namespace HelloJobFinal.Application.ViewModels
{
    public class PaginationVm<T>
    {
        public int Take { get; set; }
        public int? CategoryId { get; set; }
        public int Order { get; set; }
        public string? Search { get; set; }
        public int CurrentPage { get; set; }
        public double TotalPage { get; set; }

        public int? CategoryItemId { get; set; }
        public int? EducationId { get; set; }
        public int? ExperienceId { get; set; }
        public int? WorkingHourId { get; set; }
        public bool? HasDriverLicense { get; set; }


        public ICollection<T>? Items { get; set; }
        public T? Item { get; set; }
    }
}

