using HelloJobFinal.Domain.Entities;

namespace HelloJobFinal.Application.ViewModels
{
    public record IncludeEducationVm
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public List<Cv>? Cvs { get; init; }

    }

}

