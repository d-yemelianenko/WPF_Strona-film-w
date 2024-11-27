using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
 


namespace Strona_filmów
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MovieModel> listPlace = new List<MovieModel>();

        public MainWindow()
        {
            InitializeComponent();

        }


        private void BtnClick_Reg(object sender, RoutedEventArgs e)
        {
            string login = txtBox_Login.Text.Trim();
            string password = txtBox_Password.Password;

            // Sprawdzanie, czy pola są wypełnione
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Login i hasło nie mogą być puste!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "server=localhost;port=3306;username=root;password=;database=filmy;";
            string query = "SELECT COUNT(*) FROM users WHERE `Login` = @log AND `Password` = @pass";

            using (MySqlConnection dbConnection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, dbConnection);
                command.Parameters.Add("@log", MySqlDbType.VarChar).Value = login;
                command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password;

                try
                {
                    dbConnection.Open();
                    int userCount = Convert.ToInt32(command.ExecuteScalar()); // Sprawdzamy, czy użytkownik istnieje

                    if (userCount > 0)
                    {
                        MessageBox.Show("Logowanie zakończone sukcesem!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Przechodzimy do strony głównej
                        Glowna glowna = new Glowna();
                        glowna.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono użytkownika. Przejście do strony rejestracji.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Przeniesienie do strony rejestracji
                        Autor autor = new Autor();
                        autor.Show();
                        Close();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Błąd logowania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        private void Btn_Autoryzacja(object sender, RoutedEventArgs e)
        {
            Autor autoryzacja= new Autor();
            autoryzacja.Show();
            Close();
        }
    }
}


    

