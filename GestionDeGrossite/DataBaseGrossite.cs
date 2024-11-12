using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class DataBaseGrossite
{
    private static string connectionString = @"Data Source=" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files\dbGrossite2.db") + ";Version=3";

    public static void InitializeDataBase()
    {
        string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files\dbGrossite2.db");

        try
        {
            Console.WriteLine("Chemin vers la base de données : " + databasePath);

            // Vérifier si le fichier SQLite n'existe pas
            if (!File.Exists(databasePath))
            {
                // Créer le fichier de base de données si nécessaire
                SQLiteConnection.CreateFile(databasePath);
                Console.WriteLine("Base de données créée avec succès.");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connexion à la base de données établie.");

                    // Créer la table "clients"
                    string createClients = @"
                        CREATE TABLE IF NOT EXISTS clients (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nom TEXT NOT NULL,
                        adresse TEXT NOT NULL,
                        siret TEXT NOT NULL
                        );";

                    string createUsers = @"
                        CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL,
                        password TEXT NOT NULL
                        );";

                    string createCategorie = @"
                        CREATE TABLE IF NOT EXISTS categories (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nom TEXT NOT NULL
                        );";

                    string createProduit = @"
                        CREATE TABLE IF NOT EXISTS produits (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nom TEXT NOT NULL,
                        prix REAL NOT NULL,
                        quantite INTEGER NOT NULL,
                        categorie_id INTEGER NOT NULL,
                        FOREIGN KEY (categorie_id) REFERENCES categories(id)
                        );";

                    string createOrder = @"
                        CREATE TABLE IF NOT EXISTS orders (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        client_id INTEGER NOT NULL,
                        produit_id INTEGER NOT NULL,        
                        date_commande TEXT NOT NULL,
                        quantite INTEGER NOT NULL,
                        statut TEXT NOT NULL,
                        FOREIGN KEY (client_id) REFERENCES clients(id),
                        FOREIGN KEY (produit_id) REFERENCES produits(id)    
                        );";

                    using (var command = new SQLiteCommand(createClients, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'clients' créée avec succès.");
                    }

                    using (var command = new SQLiteCommand(createUsers, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'users' créée avec succès.");
                    }

                    using (var command = new SQLiteCommand(createCategorie, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'categories' créée avec succès.");
                    }

                    using (var command = new SQLiteCommand(createProduit, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'produits' créée avec succès.");
                    }

                    using (var command = new SQLiteCommand(createOrder, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'orders' créée avec succès.");
                    }

                    // Vérifier si la table 'clients' existe
                    string checkTableExists = "SELECT name FROM sqlite_master WHERE type='table' AND name='clients';";
                    using (var checkCommand = new SQLiteCommand(checkTableExists, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine("La table 'clients' existe dans la base de données.");
                        }
                        else
                        {
                            Console.WriteLine("La table 'clients' n'existe pas.");
                        }
                    }
                    string checkTableExists2 = "SELECT name FROM sqlite_master WHERE type='table' AND name='users';";
                    using (var checkCommand = new SQLiteCommand(checkTableExists2, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine("La table 'users' existe dans la base de données.");
                        }
                        else
                        {
                            Console.WriteLine("La table 'users' n'existe pas.");
                        }
                    }

                    string checkTableExists3 = "SELECT name FROM sqlite_master WHERE type='table' AND name='categories';";
                    using (var checkCommand = new SQLiteCommand(checkTableExists3, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine("La table 'categories' existe dans la base de données.");
                        }
                        else
                        {
                            Console.WriteLine("La table 'categories' n'existe pas.");
                        }
                    }

                    string checkTableExists4 = "SELECT name FROM sqlite_master WHERE type='table' AND name='produits';";
                    using (var checkCommand = new SQLiteCommand(checkTableExists4, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine("La table 'produits' existe dans la base de données.");
                        }
                        else
                        {
                            Console.WriteLine("La table 'produits' n'existe pas.");
                        }
                    }

                    string checkTableExists5 = "SELECT name FROM sqlite_master WHERE type='table' AND name='orders';";
                    using (var checkCommand = new SQLiteCommand(checkTableExists5, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine("La table 'orders' existe dans la base de données.");
                        }
                        else
                        {
                            Console.WriteLine("La table 'orders' n'existe pas.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Le fichier de base de données existe déjà.");
            }
        }
        catch (Exception ex)
        {
            // Afficher les erreurs si elles se produisent
            Console.WriteLine("Erreur lors de l'initialisation de la base de données : " + ex.Message);
        }
    }

    private static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool ValidateUser(string username, string password)
    {
        bool isValidUser = false;

        // Hacher le mot de passe avant de vérifier dans la base de données
        string hashedPassword = HashPassword(password);

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            // Requête SQL pour vérifier les identifiants
            string query = "SELECT * FROM users WHERE username = @username AND password = @password";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                // Paramètres pour éviter les injections SQL
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword); // Utiliser le mot de passe haché

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    // Vérifier s'il existe au moins une ligne correspondante
                    if (reader.Read())
                    {
                        isValidUser = true;
                    }
                }
            }

            conn.Close();
        }

        return isValidUser;
    }
    public static decimal GetTotalSalesAmountBetweenDates(DateTime startDate, DateTime endDate)
    {
        decimal totalSalesAmount = 0;

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            string query = @"
            SELECT SUM(p.prix * o.quantite) AS total_ventes
            FROM orders o
            JOIN produits p ON o.produit_id = p.id
            WHERE o.date_commande BETWEEN @startDate AND @endDate;";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    totalSalesAmount = Convert.ToDecimal(result);
                }
            }
        }

        return totalSalesAmount;
    }


    // Méthode pour récupérer le produit "Best Seller"
    public static List<string> GetBestSellerRanking(int topN = 5)
    {
        List<string> bestSellerRanking = new List<string>();

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            string query = $@"
            SELECT p.nom, SUM(o.quantite) AS total_vendu
            FROM orders o
            JOIN produits p ON o.produit_id = p.id
            GROUP BY p.id
            ORDER BY total_vendu DESC
            LIMIT {topN};";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productName = reader["nom"].ToString();
                        int quantitySold = Convert.ToInt32(reader["total_vendu"]);
                        bestSellerRanking.Add($"{productName} - {quantitySold} unités vendues");
                    }
                }
            }
        }

        return bestSellerRanking;
    }

    // Méthode pour récupérer le nombre de notifications de fin de stock
    public static int GetLowStockNotifications()
    {
        int notificationCount = 0;

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            string query = @"
                SELECT COUNT(*) 
                FROM produits 
                WHERE quantite < 5;"; // Seuil de fin de stock

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    notificationCount = Convert.ToInt32(result);
                }
            }
        }

        return notificationCount;
    }
}

