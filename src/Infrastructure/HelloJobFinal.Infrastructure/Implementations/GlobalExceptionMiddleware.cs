using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HelloJobFinal.Infrastructure.Implementations
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                string encodedErrorMessage = Uri.EscapeDataString(ex.Message);
                string errorPath = $"/Home/ErrorPage?error={encodedErrorMessage}";
                context.Response.Redirect(errorPath);
            }
        }
    }
}
