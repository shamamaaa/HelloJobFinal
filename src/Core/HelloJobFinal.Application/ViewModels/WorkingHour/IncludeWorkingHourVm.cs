using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record IncludWorkingHourVm
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public List<Cv>? Cvs { get; init; }

    }

}

