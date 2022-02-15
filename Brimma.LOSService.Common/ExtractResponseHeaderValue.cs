using System.Net.Http.Headers;

namespace Brimma.LOSService.Common
{
    public static class ExtractResponseHeaderValue
    {
        public static dynamic GetValueFromHttpResponseHeaders(HttpResponseHeaders httpResponseHeaders, string headerName)
        {
            dynamic headerValue = null;
            if (httpResponseHeaders != null && httpResponseHeaders.Contains(headerName))
            {
                headerValue = ((string[])httpResponseHeaders.GetValues(headerName))[0];
            }
            return headerValue;
        }
    }
}
