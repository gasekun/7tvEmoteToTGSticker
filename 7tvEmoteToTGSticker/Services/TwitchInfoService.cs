using _7tvEmoteToTGSticker.Models.TwitchModel;

namespace _7tvEmoteToTGSticker.Services;

public class TwitchInfoService
{
    private HttpClient httpClient;
    private string apiEndpoint = "https://api.ivr.fi/v2/";
    public TwitchInfoService(HttpClient client)
    {
        this.httpClient = client;
        httpClient.BaseAddress = new Uri(apiEndpoint);
    }

    public async Task<TwitchUser?> GetTwitchUserById(string id)
    {
        var requestUri = $"twitch/user?id={id}";

        var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), requestUri));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return (await response.Content.ReadFromJsonAsync<List<TwitchUser>>())!.First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
       
        }

        return null;
    }
}