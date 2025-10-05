using System.Net.Http.Json;
using System.Text.Json;
using XiletradeAuth.Models;
using XiletradeAuth.Pages.Authentication;

namespace XiletradeAuth.Services;

/// <summary>
/// Service used to recover poe token
/// </summary>
public class PoeService
{
    private readonly HttpClient _http;

    public PoeService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PoeResponseToken> GetTokenAsync()
    {
        return await _http.GetFromJsonAsync<PoeResponseToken>("sample-data/poeresponse.json");
    }

    public async Task<PoeResponseToken> GetPoeTokenAsync(string code, string codeVerifier)
    {
        try
        {
            var request = $"client_id={Poe.ClientId}&grant_type=authorization_code&code={code}&redirect_uri={Poe.RedirectUri}&scope={Poe.Scope}&code_verifier={codeVerifier}";
            var response = await _http.PostAsync(Poe.TokenUrl, new StringContent(request));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("BADREQUEST");
            }

            var responseContent = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PoeResponseToken>(responseContent);
            if (result is null || result.Token is null)
            {
                throw new JsonException("NO/BAD TOKEN");
            }

            return result;
        }
        catch (Exception) 
        { 
            throw; 
        }
    }
}
