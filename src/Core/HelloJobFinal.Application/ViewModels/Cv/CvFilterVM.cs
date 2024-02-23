
namespace HelloJobFinal.Application.ViewModels
{
	public class CvFilterVM
	{
		public List<ItemCvVm> Cvs { get; set; }
        public List<IncludeCategoryItemVm> Categories { get; set; }
		public List<IncludeCityVm> Cities { get; set; }
		public List<IncludeEducationVm> Educations { get; set; }
		public List<IncludeExperienceVm> Experiences { get; set; }
		public List<IncludWorkingHourVm> WorkingHours { get; set; }
        public List<int> CvIds { get; set; }

    }
}

