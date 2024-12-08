using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GestionDeGrossite.Model;


namespace GestionDeGrossite.Services
{
    public class CategoriesService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseUrl = "http://localhost:3200/api/categories/"; // URL de votre API

        // Méthode pour récupérer toutes les catégories depuis l'API
        public async Task<List<categories>> GetCategoriesAsync()
        {
            try
            {
                // Envoi de la requête GET à l'API pour récupérer les catégories
                HttpResponseMessage response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Lire la réponse et la convertir en liste d'objets 'categories'
                    var categoriesJson = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<categories>>(categoriesJson);
                    return categories;
                }
                else
                {
                    // Gérer l'erreur si la réponse n'est pas un succès
                    throw new Exception($"Erreur lors de la récupération des catégories : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions et afficher un message d'erreur
                throw new Exception("Une erreur est survenue lors de la connexion à l'API des catégories.", ex);
            }
        }
    }
}
