using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace Strona_filmów
{
    /// <summary>
    /// Logika interakcji dla klasy Create.xaml
    /// </summary>
    public partial class Create : Window
    {
        string photoPath;
        public Create()
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
            place.PhotoName = txtPhotoName.Text ;
           
            MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");
            
            dbConnection.Open();
            string dbQuery = "INSERT INTO filmy  VALUES (NULL, @N, @R, @A, @P)";

            MySqlCommand cmd = new MySqlCommand(dbQuery, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@N", MySqlDbType.VarChar).Value =place.Nazwa;
            cmd.Parameters.Add("@R", MySqlDbType.VarChar).Value = place.Rok;
            cmd.Parameters.Add("@A", MySqlDbType.VarChar).Value = place.Autor;
            cmd.Parameters.Add("@P", MySqlDbType.VarChar).Value = place.PhotoName;
            try
            {
               if(cmd.ExecuteNonQuery() == 1)
                { 
                MessageBox.Show("Added Successfully", "Information", MessageBoxButton.OK);
                dbConnection.Close();
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Wystąpił błąd", "Error", MessageBoxButton.OK);

            }
            

        }

              private void btnUploadPhoto_Click(object sender, RoutedEventArgs e)
              {
                  OpenFileDialog openFileDialog = new OpenFileDialog();
                  if (openFileDialog.ShowDialog() == true)
                  {
                      photoPath = openFileDialog.FileName;
                      Uri fileUri = new Uri(openFileDialog.FileName);
                      imgPhoto.Source = new BitmapImage(fileUri);
                  }
              }
    }

    
}
