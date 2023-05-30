using Newtonsoft.Json.Linq;

namespace DataverseBlazorServerApplication.Data;

using System;
using System.Net.Http;
using System.Threading.Tasks;

public class DataverseTokenService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    private string _cachedToken;
    private DateTime _tokenExpiration;
    
    public DataverseTokenService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _configuration = configuration;
        _cachedToken = null;
        _tokenExpiration = DateTime.MinValue;
    }

    public async Task<string> GetBearerToken()
    {
        // Check if token is already cached and not expired
        if (!string.IsNullOrEmpty(_cachedToken) && _tokenExpiration > DateTime.UtcNow.AddMinutes(5))
        {
            return _cachedToken;
        }
        
        var tokenEndpoint = $"https://login.windows.net/{_configuration["DataverseConfig:TenantId"]}/oauth2/token";
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        tokenRequest.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _configuration["DataverseConfig:ClientId"]),
            new KeyValuePair<string, string>("client_secret", _configuration["DataverseConfig:SecretValue"]),
            new KeyValuePair<string, string>("resource", _configuration["DataverseConfig:Url"])
        });

        try
        {
            HttpResponseMessage response = await _httpClient.SendAsync(tokenRequest);

            if (response.IsSuccessStatusCode)
            {
                // Update the cached token and expiration time
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(responseContent);
                _cachedToken = (jObject.GetValue("access_token") ?? throw new InvalidOperationException()).Value<string>();
                _tokenExpiration = DateTime.UtcNow.AddSeconds((jObject.GetValue("expires_in") ?? throw new InvalidOperationException()).Value<double>());
                return _cachedToken;
            }
            else
            {
                // Handle the error case (e.g., log, throw an exception, etc.)
                throw new Exception($"Token request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occurred during the token request
            Console.WriteLine($"Token request failed: {ex.Message}");
            throw;
        }
    }
}