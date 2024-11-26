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
            MovieModel place = new MovieModel();
            place.Nazwa = txtNazwa.Text;
            place.Rok = txtRok.Text;
            place.Autor = txtAutor.Text;
            place.PhotoName = txtNazwa.Text ;

            MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");

            dbConnection.Open();
            string dbQuery = "INSERT INTO filmy  VALUES (NULL, @N, @R, @A, @P)";

            MySqlCommand cmd = new MySqlCommand(dbQuery, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@N", MySqlDbType.VarChar).Value = place.Nazwa;
            cmd.Parameters.Add("@R", MySqlDbType.VarChar).Value = place.Rok;
            cmd.Parameters.Add("@A", MySqlDbType.VarChar).Value = place.Autor;
            cmd.Parameters.Add("@P", MySqlDbType.VarChar).Value = place.PhotoName;

            
             try
            {
                string dQuery="UPADATE Filmy SET Nazwa = '{0}', Rok = '{1}', Autor = '{2}', PhotoName = '{3} WHERE Id = {4}, place.Nazwa, place.Rok, place.Autor, place.PhotoName";
                MySqlCommand md = new MySqlCommand(dQuery, dbConnection);
                md.CommandType = CommandType.Text;

                if (cmd.ExecuteNonQuery() == 1)
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
                txtNazwa.Text = listMovie[cmbListView.SelectedIndex].Nazwa;
                txtRok.Text = listMovie[cmbListView.SelectedIndex].Rok;
                txtAutor.Text = listMovie[cmbListView.SelectedIndex].Autor;
                if (listMovie[cmbListView.SelectedIndex].PhotoName != string.Empty)
                {
                    // string photoPath = System.IO.Path.Combine(SQLiteAccess.dbFileDirectory, listMovie[cmbListView.SelectedIndex].PhotoName);
                    string path = "C:\\Users\\Dasza\\Desktop\\app filmy\\" +  listMovie[cmbListView.SelectedIndex].PhotoName;
                    Uri fileUri = new Uri(path);
                    imgPhoto.Source = new BitmapImage(fileUri);
                }
            }
        }
    }

}
