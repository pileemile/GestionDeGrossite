using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using GestionDeGrossite.Model;

namespace GestionDeGrossite
{
    public partial class GestionProduits : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public GestionProduits()
        {
            InitializeComponent();
            LoadCategories();
        }

        // Charger les catégories dans le ComboBox
        private async void LoadCategories()
        {
            try
            {
               
                HttpResponseMessage response = await client.GetAsync("http://localhost:3200/api/categories/");

                if (response.IsSuccessStatusCode)
                {
                    var categoriesJson = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<categories>>(categoriesJson);

                  
                    CategoryComboBox.ItemsSource = categories;
                    CategoryComboBox.DisplayMemberPath = "Nom"; 
                    CategoryComboBox.SelectedValuePath = "Id";  
                }
                else
                {
                    MessageBox.Show($"Erreur lors du chargement des catégories : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}");
            }
        }

        // Méthode d'ajout de produit via l'API
        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
           
            string productName = ProductNameTextBox.Text;
            string productPrice = ProductPriceTextBox.Text;
            int quantity = int.Parse(ProductQuantityTextBox.Text);
            DateTime? expirationDate = ExpirationDatePicker.SelectedDate;
            string location = LocationTextBox.Text;

            

       
            var selectedCategory = (categories)CategoryComboBox.SelectedItem;

         
            var newProduct = new
            {
                Nom = productName,
                Prix = productPrice,
                Quantite = quantity,
                DatePeremtion = expirationDate,
                Emplacement = location,
                Categorie = selectedCategory.Id 
            };

            // Sérialiser l'objet en JSON
            var jsonContent = JsonConvert.SerializeObject(newProduct);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Envoyer la requête POST à l'API pour ajouter le produit
                HttpResponseMessage response = await client.PostAsync("http://localhost:3200/api/produits/", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Produit ajouté avec succès !");
                    // Rafraîchir la liste des produits (si nécessaire)
                    RefreshProductList();
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur : {response.ReasonPhrase}\nDétails : {responseContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}");
            }
        }



        // Rafraîchir la liste des produits après ajout (facultatif)
        private async void RefreshProductList()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:3200/api/produits/"); 

                if (response.IsSuccessStatusCode)
                {
                    var productsJson = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<produits>>(productsJson);

                  
                    ProductList.ItemsSource = products;
                }
                else
                {
                    MessageBox.Show($"Erreur lors du chargement des produits : {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}");
            }
        }
    }
}
