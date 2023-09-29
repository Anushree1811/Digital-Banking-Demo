using Demo.Model;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostalInfoController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PostalInfoController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("{pincode}")]
    public async Task<ActionResult<PostOffice[]>> Get(string pincode, string? name = null)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            // Send a GET request to the provided API endpoint
            var response = await client.GetAsync($"https://api.postalpincode.in/pincode/{pincode}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content
                var content = await response.Content.ReadAsStringAsync();

                // You can use a JSON library like Newtonsoft.Json to parse the response
                // Here, I'm using the built-in System.Text.Json
                var result = System.Text.Json.JsonSerializer.Deserialize<PostalCodeResponse[]>(content);

                if (result.Length > 0)
                {
                    if (name != null) {
                        var postOffice = result[0].PostOffice;

                        // Filter the PostOffice array based on both pincode and name
                        var filteredPostOffice = postOffice
                            .Where(po => po.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                            .Select(po => new PostOffice
                            {
                                Name = po.Name,
                                Circle = po.Circle,
                                Region = po.Region
                            })
                            .ToList();

                        if (filteredPostOffice.Count > 0)
                        {
                            return Ok(filteredPostOffice); // Return the filtered results
                        }
                        else {

                            return StatusCode(500, $"PostOffice with name {name} not found");
                        }
                    }
                    return Ok(result[0].PostOffice); // Return the first result
                }
                else
                {
                    return NotFound("Postal code not found.");
                }
            }
            else
            {
                return BadRequest("Failed to retrieve data from the API.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

