using System.Windows;

namespace GestionDeGrossite
{
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
            // Récupération des valeurs saisies par l'utilisateur
            string username = UsernameTextBox.Text; // TextBox du nom d'utilisateur
            string password = PasswordBox.Password; // PasswordBox du mot de passe

            // Vérification des identifiants
            if (DataBaseGrossite.ValidateUser(username, password))
            {
                // Ouvrir la fenêtre du tableau de bord si la validation est réussie
                Dashbord dashbordWindow = new Dashbord();
                dashbordWindow.Show();

                // Fermer la fenêtre de connexion
                this.Close();
            }
            else
            {
                // Affichage d'un message d'erreur si les identifiants sont incorrects
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}