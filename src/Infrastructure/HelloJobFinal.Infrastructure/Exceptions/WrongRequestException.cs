﻿namespace HelloJobFinal.Infrastructure.Exceptions
{
    public class WrongRequestException : Exception
    {
        public WrongRequestException(string message) : base(message) { }
    }
}
