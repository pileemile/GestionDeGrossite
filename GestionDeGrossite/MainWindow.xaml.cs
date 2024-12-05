using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace GestionDeGrossite
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var result = await AuthenticateUser(username, password);

            if (result.IsAuthenticated)
            {
                // Enregistrez le token pour une utilisation ultérieure si nécessaire
                Console.WriteLine($"Token d'accès : {result.AccessToken}");

                Dashbord dashbordWindow = new Dashbord();
                dashbordWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<LoginResult> AuthenticateUser(string username, string password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string apiUrl = "http://localhost:3200/api/users/login/";

                    var loginData = new
                    {
                        username = username,
                        password = password
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);

                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Réponse complète : " + responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        // Désérialisez la réponse pour obtenir les tokens
                        var jsonResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);

                        return new LoginResult
                        {
                            IsAuthenticated = true,
                            AccessToken = jsonResponse.Access,
                            RefreshToken = jsonResponse.Refresh
                        };
                    }
                    else
                    {
                        // Gérez les erreurs de manière plus détaillée
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                        return new LoginResult
                        {
                            IsAuthenticated = false,
                            ErrorMessage = errorResponse?.Detail ?? "Erreur inconnue lors de l'authentification."
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de l'authentification : " + ex.Message);
                    return new LoginResult
                    {
                        IsAuthenticated = false,
                        ErrorMessage = "Une erreur s'est produite. Veuillez vérifier votre connexion réseau."
                    };
                }
            }
        }
    }

    // Classes auxiliaires pour la gestion des réponses
    public class LoginResponse
    {
        [JsonProperty("access")]
        public string Access { get; set; }

        [JsonProperty("refresh")]
        public string Refresh { get; set; }
    }

    public class ErrorResponse
    {
        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class LoginResult
    {
        public bool IsAuthenticated { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
