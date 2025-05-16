namespace GylleneDroppen.Blazor.Client.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient EnableCookies(this HttpClient client)
    {
        client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        client.DefaultRequestHeaders.Add("withCredentials", "true");
        return client;
    }
}