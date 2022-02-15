using Brimma.LOSService.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Brimma.LOSService.Extensions
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly CultureInfo EnUsCultureInfoProvider = new CultureInfo("en-us");
        private readonly ILogger<CustomAuthorizationMiddleware> logger;
        private readonly IApplicationInsights applicationInsights;

        public CustomAuthorizationMiddleware(RequestDelegate next, ILogger<CustomAuthorizationMiddleware> logger, IApplicationInsights applicationInsights)
        {
            _next = next;
            this.logger = logger;
            this.applicationInsights = applicationInsights;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context != null)
            {
                if (context.Request.Headers.Keys.Count > 0 && context.Request.Headers.ContainsKey("Authorization"))
                {
                    bool authorized = ValidateAndExtactToken(context.Request.Headers);
                    if (authorized)
                    {
                        await _next.Invoke(context).ConfigureAwait(false);
                        return;
                    }
                }
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized!").ConfigureAwait(false);
            }
        }
        private bool ValidateAndExtactToken(IHeaderDictionary headerDictionary)
        {
            StringValues authorizationValues = headerDictionary["Authorization"];
            if (authorizationValues.Count > 0 && !string.IsNullOrEmpty(authorizationValues[0]))
            {
                string[] authValues = authorizationValues[0].Split(' ');
                if (authValues != null && authValues.Length == 2)
                {
                    AppData.AuthResponse = new Brimma.LOSService.DTO.AuthResponse();
                    AppData.AuthResponse.TokenType = authValues[0];
                    AppData.AuthResponse.AccessToken = authValues[1];
                    logger.LogDebug("Auth token response", JsonConvert.SerializeObject(AppData.AuthResponse));
                    applicationInsights.AddCustomPropertiesInAppInsights("CustomAuthorizationMiddleware", "ValidateAndExtactToken", AppData.AuthResponse, null, "Auth token", "", "");
                    return true;
                }
            }
            return false;
        }
    }
}