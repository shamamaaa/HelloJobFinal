﻿using System;
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
        public ICollection<T>? Items { get; set; }
        public T? Item { get; set; }
    }
}
