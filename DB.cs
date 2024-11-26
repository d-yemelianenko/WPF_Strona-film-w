using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Drawing;

using System.Diagnostics;
using Microsoft.Data.Sqlite;
using System.Data;
using MySql.Data.MySqlClient;

namespace Strona_filmów
{
    class DB
    {
        public static string dbFileDirectory = Environment.CurrentDirectory;
        private static string dbFilePath = System.IO.Path.Combine(dbFileDirectory, "Test.db");
        private static string _connectionString = string.Format("Data Source = {0}", dbFilePath);

        
        // Read
        public static List<MovieModel> Read()
        {
            List<MovieModel> movieList = new List<MovieModel>();

            MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");

            string dbQuery = "SELECT * FROM filmy";
            dbConnection.Open();
            try
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);

                    MySqlDataReader dr = dbCommand.ExecuteReader();


                    while (dr.Read())
                    {
                        MovieModel place = new MovieModel()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nazwa = dr["Nazwa"].ToString(),
                            Rok = dr["Rok"].ToString(),
                            Autor = dr["Autor"].ToString(),
                            PhotoName = dr["PhotoName"].ToString()


                        };
                        movieList.Add(place);
                    }
                    
                    dr.Close();

                }
            }
            catch (Exception ex)
            {
                dbConnection.Close();

            }
            return movieList;

        }


        public static bool Create(MovieModel item)
        {
            bool isCreated = false;

            MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");

            try
            {
                dbConnection.Open();
                if (dbConnection.State == ConnectionState.Open)
                {
                    string dbQuery = string.Format("SELECT COUNT(Id) FROM filmy WHERE Nazwa = '{0}'", item.Nazwa);
                    MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);

                    MySqlDataReader dr = dbCommand.ExecuteReader();

                    while (dbCommand.ExecuteNonQuery() == 1)
                    {

                        dbQuery = string.Format("INSERT INTO filmy VALUES( NULL,'{0}', '{1}', '{2}', '{3}' )", item.Nazwa, item.Rok, item.Autor, item.PhotoName);
                        dbCommand.CommandText = dbQuery;
                        if (dbCommand.ExecuteNonQuery() == 0)
                        {
                            isCreated = true;
                        };

                    }

                    dr.Close();

                }
            }

            catch (Exception ex)
            {
                dbConnection.Close();

            }
           return isCreated;

        }

                    
                       
        

        public static bool Delete(MovieModel item)
        {
            bool isDeleted = false;
            MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");
            dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open)
            {
                string dbQuery = string.Format("SELECT COUNT(Id) FROM filmy WHERE Nazwa = '{0}'", item.Nazwa);
                MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);

                long result = (long)dbCommand.ExecuteScalar();
                if (result == 1)
                {
                    dbQuery = string.Format("DELETE FROM filmy WHERE Nazwa = '{0}'", item.Nazwa);
                    dbCommand.CommandText = dbQuery;

                    if (dbCommand.ExecuteNonQuery() == 1)
                    {
                        isDeleted = true;
                    }
                }
            }
            dbConnection.Close();
            return isDeleted;
        }

        public static bool Update(MovieModel item)
        {
            bool isUpdated = false;
            
            MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");

            try
            {
                dbConnection.Open();
                if (dbConnection.State == ConnectionState.Open)
                {
                    string dbQuery = string.Format("SELECT COUNT(Id) FROM filmy WHERE Nazwa = '{0}'", item.Nazwa);
                    MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);              
                    MySqlDataReader dr = dbCommand.ExecuteReader();

                    while (dbCommand.ExecuteNonQuery() == 1)
                    { 
                        
                    dbQuery = string.Format("UPADATE Filmy SET Nazwa = '{0}', Rok = '{1}', Autor = '{2}', PhotoName = '{3} WHERE Id = {4}", item.Nazwa, item.Rok, item.Autor, item.PhotoName);
                    dbCommand.CommandText = dbQuery;

                        
                        if (dbCommand.ExecuteNonQuery() == 0)
                        {
                            isUpdated = true;
                        };

                    }

                    dr.Close();

                }
            }

            catch (Exception ex)
            {
                dbConnection.Close();

            }
            
            return isUpdated;

        }
                
           
    }


}

