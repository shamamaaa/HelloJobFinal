namespace HelloJobFinal.Infrastructure.Exceptions
{
    public class WrongRequestException : Exception
    {
        public WrongRequestException(string message = "Oops, yanlış məlumat yenidən cəhd edin :'(") : base(message) { }
    }
}
