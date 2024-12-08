using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GestionDeGrossite.Model;
using GestionDeGrossite.Service;
using GestionDeGrossite.Services;
using LiveCharts;
using LiveCharts.Wpf;

namespace GestionDeGrossite
{
    public partial class Dashbord : Window
    {
        private readonly OrderService _orderService;
        private readonly ProduitService _productService;
        public ChartValues<decimal> BarChartSeries { get; set; } = new ChartValues<decimal>();
        public string[] BarChartLabels { get; set; }
        public SeriesCollection PieChartSeries { get; set; } = new SeriesCollection();

        public Dashbord()
        {
            InitializeComponent();
            _orderService = new OrderService();
            _productService = new ProduitService();
            PieChartSeries = new SeriesCollection();
            BarChartLabels = new string[] { };

            DataContext = this; 
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync();
                var products = await _productService.GetProductsAsync();

                
                DisplayTotalSales(orders, products);
                DisplayTopProducts(orders, products);
                DisplaySalesStatistics(orders, products); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private void DisplayTotalSales(List<orders> orders, List<produits> products)
        {
            // Calculer le montant total par produit
            var salesByProduct = orders
                .Join(products,
                    o => o.Produit,
                    p => p.Id,
                    (o, p) => new
                    {
                        Produit = p.Nom,
                        MontantTotal = o.Quantite * decimal.Parse(p.Prix, CultureInfo.InvariantCulture)
                    })
                .GroupBy(x => x.Produit)
                .Select(g => new
                {
                    Produit = g.Key,
                    MontantTotal = g.Sum(x => x.MontantTotal)
                })
                .OrderByDescending(x => x.MontantTotal)
                .ToList();

            // Afficher les montants dans le ListView
            productSalesListView.ItemsSource = salesByProduct;
        }

        private void DisplayTopProducts(List<orders> orders, List<produits> products)
        {
            // Calculer les quantités vendues par produit
            var productSales = orders
                .GroupBy(o => o.Produit)
                .Select(g => new { ProduitId = g.Key, QuantiteTotale = g.Sum(o => o.Quantite) })
                .OrderByDescending(p => p.QuantiteTotale)
                .ToList();

            
            var productNamesWithSales = productSales
                .Join(products,
                    ps => ps.ProduitId,
                    p => p.Id,
                    (ps, p) => new { Nom = p.Nom, ps.QuantiteTotale })
                .ToList();

            // Afficher les produits dans le TextBlock
            bestSellerTextBlock.Text = string.Join(Environment.NewLine,
                productNamesWithSales.Select(ps => $"{ps.Nom}: {ps.QuantiteTotale} ventes"));
        }

        private void DisplaySalesStatistics(List<orders> orders, List<produits> products)
        {
            // Préparer les données groupées par produit
            var salesData = orders
                .Join(products,
                    o => o.Produit,
                    p => p.Id,
                    (o, p) => new
                    {
                        Produit = p.Nom,
                        MontantTotal = o.Quantite * decimal.Parse(p.Prix, CultureInfo.InvariantCulture)
                    })
                .GroupBy(x => x.Produit)
                .Select(g => new
                {
                    Produit = g.Key,
                    MontantTotal = g.Sum(x => x.MontantTotal)
                })
                .ToList();

            /
            BarChartLabels = salesData.Select(x => x.Produit).ToArray(); 
            BarChartSeries.Clear();
            BarChartSeries.AddRange(new ChartValues<decimal>(salesData.Select(x => x.MontantTotal))); 

            // Mise à jour des données pour le graphique circulaire
            PieChartSeries.Clear();
            foreach (var data in salesData)
            {
                PieChartSeries.Add(new PieSeries
                {
                    Title = data.Produit,
                    Values = new ChartValues<decimal> { data.MontantTotal },
                    DataLabels = true
                });
            }
        }

        private void OpenGestionProduits_Click(object sender, RoutedEventArgs e)
        {
            GestionProduits gestionProduitsWindow = new GestionProduits();
            gestionProduitsWindow.Show();
        }
    }
}
