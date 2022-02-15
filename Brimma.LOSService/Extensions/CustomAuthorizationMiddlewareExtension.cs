using Microsoft.AspNetCore.Builder;
using System;

namespace Brimma.LOSService.Extensions
{
    public static class CustomAuthorizationMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<CustomAuthorizationMiddleware>();
        }
    }
}