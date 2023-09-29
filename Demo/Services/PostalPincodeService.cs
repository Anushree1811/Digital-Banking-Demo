using Demo.Model;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Demo.Services;

public class PostalPincodeService
{
    private readonly HttpClient _httpClient;

    public PostalPincodeService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PostalInfo> GetPostalInfo(string pincode)
    {
        var response = await _httpClient.GetFromJsonAsync<PostalInfo>($"pincode/{pincode}");
        return response;
    }
}
