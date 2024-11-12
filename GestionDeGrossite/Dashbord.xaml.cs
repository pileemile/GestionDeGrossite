using System;
using System.Collections.Generic;
using System.Windows;

namespace GestionDeGrossite
{
    public partial class Dashbord : Window
    {
        public Dashbord()
        {
            InitializeComponent();
            DisplayDashboardData();
        }

        private void DisplayDashboardData()
        {
            // Définir la période : ventes de la dernière année
            DateTime startDate = DateTime.Now.AddYears(-1);
            DateTime endDate = DateTime.Now;

            // Récupérer les données depuis la base de données
            decimal totalSalesAmount = DataBaseGrossite.GetTotalSalesAmountBetweenDates(startDate, endDate);
            List<string> bestSellers = DataBaseGrossite.GetBestSellerRanking();
            int lowStockNotifications = DataBaseGrossite.GetLowStockNotifications();

            // Afficher les valeurs sur le tableau de bord
            totalSalesTextBlock.Text = $"Montant total des ventes (dernière année) : {totalSalesAmount:C}";
            bestSellerTextBlock.Text = "Top des produits :\n" + string.Join("\n", bestSellers);
            notificationsTextBlock.Text = $"Notifications de fin de stock : {lowStockNotifications}";
        }

        private void OpenGestionProduits_Click(object sender, RoutedEventArgs e)
        {
            GestionProduits gestionProduitsWindow = new GestionProduits();
            gestionProduitsWindow.Show();
        }
    }
}