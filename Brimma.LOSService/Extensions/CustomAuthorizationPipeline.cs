using Microsoft.AspNetCore.Builder;

namespace Brimma.LOSService.Extensions
{
    public class CustomAuthorizationPipeline
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCustomAuthorization();
        }
    }
}