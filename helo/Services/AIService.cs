using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace helo.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["APIKeys:Gemini"];
        }

        //public async Task<string> GetRestockingPrediction(string salesData)
        //{
        //    var requestBody = new
        //    {
        //        prompt = $"Analyze this sales data and predict restocking needs: {salesData}",
        //        max_tokens = 100
        //    };

        //    var jsonRequest = JsonSerializer.Serialize(requestBody);
        //    var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        //    var response = await _httpClient.PostAsync("https://api.openai.com/v1/completions", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsonResponse = await response.Content.ReadAsStringAsync();
        //        var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
        //        return result.GetProperty("choices")[0].GetProperty("text").GetString();
        //    }
        //    else
        //    {
        //        return "AI response failed.";
        //    }
        //}
        public async Task<string> GetRestockingPrediction(string salesData)
        {
            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = $"Analyze this sales data and predict restocking needs: {salesData}" }
                }
            }
        }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                return result.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
            }
            else
            {
                return "AI response failed.";
            }
        }

    }
}
