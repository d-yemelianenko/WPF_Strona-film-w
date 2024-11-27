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
using Microsoft.Win32;
using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.Cosmos;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Strona_filmów
{
    /// <summary>
    /// Logika interakcji dla klasy Glowna.xaml
    /// </summary>
    public partial class Glowna : Window
    {
        List<MovieModel> listMovie = new List<MovieModel>();
        string photoPath;

        public Glowna()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Create newWindow = new Create();
            newWindow.ShowDialog();
            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbListView.SelectedItem == null)
            {
                MessageBox.Show("Wybierz film do edycji.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MovieModel place = listMovie[cmbListView.SelectedIndex]; // Pobierz wybrany film
            place.Nazwa = txtNazwa.Text;
            place.Rok = txtRok.Text;
            place.Autor = txtAutor.Text;

            string connectionString = "server=localhost;port=3306;username=root;password=;database=filmy;";
            string updateQuery = "UPDATE filmy SET Nazwa = @N, Rok = @R, Autor = @A WHERE Id = @Id";

            using (MySqlConnection dbConnection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, dbConnection))
                {
                    cmd.Parameters.Add("@N", MySqlDbType.VarChar).Value = place.Nazwa;
                    cmd.Parameters.Add("@R", MySqlDbType.VarChar).Value = place.Rok;
                    cmd.Parameters.Add("@A", MySqlDbType.VarChar).Value = place.Autor;
                    cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = place.Id;

                    try
                    {
                        dbConnection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Film został zaktualizowany pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Nie udało się zaktualizować filmu. Sprawdź dane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       

        private void cmbListView_DropDownOpened(object sender, EventArgs e)
        {
            listMovie = DB.Read();
            cmbListView.ItemsSource = listMovie.Select(n => n.Nazwa);
        }


     
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (cmbListView.SelectedItem != null)
            {
                if (MessageBox.Show("Jesteś pewien?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (DB.Delete(listMovie[cmbListView.SelectedIndex]) == false)
                    {
                         cmbListView.SelectedIndex--;
                    }
                    else
                    {
                       MessageBox.Show("Wystąpił błąd", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }
        private void cmbListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbListView.SelectedItem != null)
            {
                // Pobieranie danych wybranego filmu
                txtNazwa.Text = listMovie[cmbListView.SelectedIndex].Nazwa;
                txtRok.Text = listMovie[cmbListView.SelectedIndex].Rok;
                txtAutor.Text = listMovie[cmbListView.SelectedIndex].Autor;

                // Wyświetlanie zdjęcia
              
                        // Ścieżka do zdjęcia
                        string photoPath = System.IO.Path.Combine("C:\\Users\\Dasha\\Desktop\\app filmy\\", listMovie[cmbListView.SelectedIndex].PhotoName);

                        // Sprawdzanie, czy plik istnieje
                        if (File.Exists(photoPath))
                        {
                            imgPhoto.Source = new BitmapImage(new Uri(photoPath, UriKind.Absolute));
                        }
                        else
                        {
                            imgPhoto.Source = null; // Usuwamy obraz, jeśli pliku nie ma
                            MessageBox.Show($"Zdjęcie '{listMovie[cmbListView.SelectedIndex].PhotoName}' nie istnieje w katalogu.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
         
            }
        }

    }





}
