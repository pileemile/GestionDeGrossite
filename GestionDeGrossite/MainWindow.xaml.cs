using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionDeGrossite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataBaseGrossite.InitializeDataBase();
         
        }

            // Méthode appelée lorsque l'utilisateur clique sur le bouton de connexion
            private void LoginButton_Click(object sender, RoutedEventArgs e)
            {
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;

                // Simulez la vérification des identifiants (vous pouvez utiliser une base de données)
                if (IsValidUser(username, password))
                {
                    // Si la connexion réussie, ouvrir la fenêtre principale
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close(); // Ferme la fenêtre de connexion
                }
                else
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Méthode qui valide les informations d'identification
            private bool IsValidUser(string username, string password)
            {
                // Remplacez cette logique par une véritable validation, par exemple, en vérifiant une base de données.
                return username == "admin" && password == "password123";
            }
        }
    }


