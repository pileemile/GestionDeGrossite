using System.Windows;
using System.Windows.Controls;

namespace GestionDeGrossite
{
    public partial class GestionProduits : Window
    {
        public GestionProduits()
        {
            InitializeComponent();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            // Créer une nouvelle ligne de produit
            StackPanel productPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            // Créer les éléments de la nouvelle ligne
            TextBox productNameTextBox = new TextBox
            {
                Width = 200,
                Height = 30,
                Margin = new Thickness(5),
                Text = "Nom du produit"
            };

            Button addButton = new Button
            {
                Content = "Ajouter",
                Width = 100,
                Height = 30,
                Margin = new Thickness(5)
            };

            Button modifyButton = new Button
            {
                Content = "Modifier",
                Width = 100,
                Height = 30,
                Margin = new Thickness(5)
            };

            Button deleteButton = new Button
            {
                Content = "Supprimer",
                Width = 100,
                Height = 30,
                Margin = new Thickness(5)
            };

            // Ajouter les éléments dans le StackPanel
            productPanel.Children.Add(productNameTextBox);
            productPanel.Children.Add(addButton);
            productPanel.Children.Add(modifyButton);
            productPanel.Children.Add(deleteButton);

            // Ajouter le StackPanel dans l'ItemsControl (ProductList)
            ProductList.Items.Add(productPanel);
        }
    }
}