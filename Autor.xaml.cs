using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Strona_filmów
{

    /// <summary>
    /// Logika interakcji dla klasy Autor.xaml
    /// </summary>
    public partial class Autor : Window
    {
        List<MovieModel> listPlace = new List<MovieModel>();
        public Autor()
        {
            InitializeComponent();

        }    

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzanie, czy oba pola hasła są zgodne
            if (Password.Password != Password2.Password)
            {
                MessageBox.Show("Hasła nie są zgodne. Spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdzanie, czy login i hasło są wypełnione
            if (string.IsNullOrWhiteSpace(Login.Text) || string.IsNullOrWhiteSpace(Password.Password))
            {
                MessageBox.Show("Login i hasło nie mogą być puste.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Łączenie się z bazą danych
            string connectionstring = "server=localhost;port=3306;username=root;password=;database=filmy;";
            string query = "INSERT INTO users (`Login`, `Password`) VALUES (@log, @pass)";

            using (MySqlConnection dbconnection = new MySqlConnection(connectionstring))
            {
                MySqlCommand command = new MySqlCommand(query, dbconnection);
                command.Parameters.Add("@log", MySqlDbType.VarChar).Value = Login.Text;
                command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = Password.Password;

                try
                {
                    dbconnection.Open();
                    command.ExecuteNonQuery(); // Wykonujemy zapytanie SQL (bez użycia `MySqlDataReader`, bo nie odczytujemy danych)

                    MessageBox.Show("Rejestracja zakończona sukcesem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Przechodzimy do głównego okna po rejestracji
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close(); // Zamykamy aktualne okno
                }
                catch (MySqlException ex)
                {
                    // Obsługa wyjątków bazy danych
                    MessageBox.Show($"Wystąpił błąd podczas rejestracji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Zamykanie połączenia w razie potrzeby
                    if (dbconnection.State == System.Data.ConnectionState.Open)
                    {
                        dbconnection.Close();
                    }
                }
            }
        }

    }

}




