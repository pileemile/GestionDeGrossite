using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GestionDeGrossite.Model;

namespace GestionDeGrossite.Service
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<orders>> GetOrdersAsync()
        {
            string apiUrl = "http://localhost:3200/api/orders/";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<orders>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return new List<orders>();
        }
    }
}