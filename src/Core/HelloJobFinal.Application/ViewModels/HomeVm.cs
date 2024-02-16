using System;
namespace HelloJobFinal.Application.ViewModels
{
	public class HomeVm
	{
		public ICollection<ItemCategoryItemVm> CategoryItemVms { get; set; }
        public ICollection<ItemBaseCategoryVm> BaseCategoryVms { get; set; }

        //public ICollection<ItemVacancyVm> VacancyVms { get; set; }
        //public ICollection<ItemCompanyVm> CompanyVms { get; set; }

    }
}

