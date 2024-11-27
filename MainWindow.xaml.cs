using MySql.Data.MySqlClient;
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

        private void Btn_Strona(object sender, RoutedEventArgs e)
        {
            Glowna autor = new Glowna();
            autor.Show();
            
        }

        private void BtnClick_Reg(object sender, RoutedEventArgs e)
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
               
                MessageBox.Show("Rigistracja jest");
                dbconnection.Close();
                Glowna autor = new Glowna();
                autor.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bląd registracji");
                Autor autor = new Autor();
                autor.Show();
                Hide();
            }
              
        }

        private void Btn_Regist(object sender, RoutedEventArgs e)
        {
            Autor autor = new Autor();
            autor.Show();

        }

    }
      
}


    

