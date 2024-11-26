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
       
        public Autor()
        { 
            InitializeComponent();

            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connectionstring = "server=localhost;port=3306;username=root;password=;database=films;";
            string query = "INSERT INTO users ( `Login`, `Password`) VALUES ( @log,@pass)";

            MySqlConnection dbconnection = new MySqlConnection(connectionstring);
            MySqlCommand command = new MySqlCommand(query, dbconnection);

            command.Parameters.Add("@log", MySqlDbType.VarChar).Value = txtBox_Login.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = txtBox_Password.Password;
            Autor aut = new Autor();

            try
            {
                dbconnection.Open();
                MySqlDataReader myReader = command.ExecuteReader();
                MessageBox.Show("Autoryzacja jest");
                dbconnection.Close();
                Glowna autor = new Glowna();
                autor.Show();
                Hide();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bląd autoryzacji");             
                MainWindow autor = new MainWindow();
                autor.Show();
                Hide();
            }
            aut.Close();
        }
          
       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
    

    
       
            



}




