namespace HelloJobFinal.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Oops, məlumat tapılmadı yenidən cəhd edin :'(") : base(message) { }
    }
}
