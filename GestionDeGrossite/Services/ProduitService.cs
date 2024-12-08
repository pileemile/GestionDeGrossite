using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GestionDeGrossite.Model;

namespace GestionDeGrossite.Services
{
    public class ProduitService
    {
        private readonly HttpClient _httpClient;

        public ProduitService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<produits>> GetProductsAsync()
        {
            string apiUrl = "http://localhost:3200/api/produits/"; 
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<produits>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return new List<produits>();
        }
    }
}
