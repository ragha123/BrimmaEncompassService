using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

/*Solution for 415 Unsupported Media Type error*/
public class NoCharSetJsonMediaTypeFormatter : JsonMediaTypeFormatter
{
    public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
    {
        base.SetDefaultContentHeaders(type, headers, mediaType);
        headers.ContentType.CharSet = "";
    }
}

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> PostAsJsonWithNoCharSetAsync<T>(this HttpClient client, string requestUri, T value, CancellationToken cancellationToken)
    {
        return await client.PostAsync(requestUri, value, new NoCharSetJsonMediaTypeFormatter(), cancellationToken);
    }

    public static async Task<HttpResponseMessage> PostAsJsonWithNoCharSetAsync<T>(this HttpClient client, string requestUri, T value)
    {
        return await client.PostAsync(requestUri, value, new NoCharSetJsonMediaTypeFormatter());
    }
}