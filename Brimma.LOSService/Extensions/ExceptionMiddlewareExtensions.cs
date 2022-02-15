using Microsoft.AspNetCore.Builder;

namespace Brimma.LOSService.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
