namespace HelloJobFinal.Application.ViewModels
{
    public record ItemEducationVm(int Id,string Name)
    {
        public ICollection<IncludeCvVm> IncludeCvVms { get; init; }
    }

}

