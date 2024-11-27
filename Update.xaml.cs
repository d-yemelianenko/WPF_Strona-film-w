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
using MySql.Data.MySqlClient;

namespace Strona_filmów
{
    /// <summary>
    /// Logika interakcji dla klasy Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        string photoPath = "";

        public Update()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MovieModel place = new MovieModel();
            place.Nazwa = txtNazwa.Text;
            place.Rok = txtRok.Text;
            place.Autor = txtAutor.Text;

            // Use the filename stored during the upload photo process
            string photoFileName = string.IsNullOrEmpty(photoPath) ? " " : System.IO.Path.GetFileName(photoPath);

            // If no photo is uploaded, keep the existing photo name
            if (string.IsNullOrEmpty(photoFileName))
            {
                photoFileName = System.IO.Path.GetFileName(photoPath); // Keep the old photo if no new one is uploaded
            }

            string connectionString = "server=localhost;port=3306;username=root;password=;database=filmy";

            using (MySqlConnection dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                string updateQuery = "UPDATE filmy SET Nazwa = @N, Rok = @R, Autor = @A, PhotoName = @P WHERE Nazwa = @Nazwa"; // Use 'Nazwa' to identify the record

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, dbConnection))
                {
                    cmd.Parameters.Add("@N", MySqlDbType.VarChar).Value = place.Nazwa;
                    cmd.Parameters.Add("@R", MySqlDbType.VarChar).Value = place.Rok;
                    cmd.Parameters.Add("@A", MySqlDbType.VarChar).Value = place.Autor;
                    cmd.Parameters.Add("@P", MySqlDbType.VarChar).Value = photoFileName; // Insert photo filename
                    cmd.Parameters.Add("@Nazwa", MySqlDbType.VarChar).Value = place.Nazwa; // Use 'Nazwa' to match the movie

                    try
                    {
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Film został zmieniony pomyślnie.", "Sukces", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("Wystąpił błąd podczas zmiany informacji o filmie.", "Błąd", MessageBoxButton.OK);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void btnUploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            // Okno dialogowe do wyboru pliku
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki obrazów|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                // Zapisujemy wybraną ścieżkę zdjęcia w zmiennej photoPath
                photoPath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(photoPath);  // Get file name with extension

                // Ustawiamy wybrany obraz
                imgPhoto.Source = new BitmapImage(new Uri(photoPath));
            }
        }
    }
}


