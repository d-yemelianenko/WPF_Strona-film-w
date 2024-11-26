using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Strona_filmów
{
    internal class SQLiteAccess
    {
        public static string dbFileDirectory = Environment.CurrentDirectory;
        private static string dbFilePath = System.IO.Path.Combine(dbFileDirectory, "Test.db");
        private static string _connectionString = string.Format("Data Source = {0}", dbFilePath);
       

        MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=films");
        
        public void openConnection()
                    {
                    
                        dbConnection.Open();
                    }

        public void closeConnection()
        {
            
                dbConnection.Close();
        }

        public MySqlConnection getConnection()
        {
            return dbConnection;
        }


        // Read
         

          
        public static List<MovieModel> Read()
        {
            List<MovieModel> listMovie = new List<MovieModel>();
                MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=filmy");

                dbConnection.Open();
            if (dbConnection.State == ConnectionState.Open)
            {
                string dbQuery = "SELECT * FROM filmy";
                MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);

                MySqlDataReader dr = dbCommand.ExecuteReader();

                while (dr.Read())
                {
                   MovieModel films = new MovieModel()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nazwa = dr["Nazwa"].ToString(),
                        Rok = dr["Rok"].ToString(),
                        Autor = dr["Autor"].ToString(),
                        PhotoName = dr["PhotoName"].ToString()
                    };
                    listMovie.Add(films);
                }
            }
            dbConnection.Close();
            return listMovie;
        }

        public static bool Create(MovieModel item)
      {
          bool isCreated = false;
          MySqlConnection dbConnection = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=filmy");
          dbConnection.Open();
          if (dbConnection.State == ConnectionState.Open)
          {
              string dbQuery = string.Format("SELECT COUNT(Id) FROM filmy WHERE Nazwa = '{0}'", item.Nazwa);
              MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);

              long result = (long)dbCommand.ExecuteScalar();
              if (result == 0)
              {
                  dbQuery = string.Format("INSERT INTO filmy VALUES(NULL, '{0}', '{1}', '{2}', '{3}' )", item.Nazwa, item.Rok, item.Autor, item.PhotoName);
                  dbCommand.CommandText = dbQuery;
                  if (dbCommand.ExecuteNonQuery() == 1)
                  {
                      isCreated = true;
                  }
              }
          }
          dbConnection.Close();
          return isCreated;
      }

         public static bool Delete(MovieModel item)
         {
             bool isDeleted = false;
             MySqlConnection db2Connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=filmy");
             db2Connection.Open();
             if (db2Connection.State == ConnectionState.Open)
             {
                 string dbQuery = string.Format("SELECT COUNT(Id) FROM Filmy WHERE Nazwa = '{0}'", item.Nazwa);
                 MySqlCommand dbCommand = new MySqlCommand(dbQuery, db2Connection);

                 long result = (long)dbCommand.ExecuteScalar();
                 if (result == 1)
                 {
                     dbQuery = string.Format("DELETE FROM Filmy WHERE Nazwa = '{0}'", item.Nazwa);
                     dbCommand.CommandText = dbQuery;

                     if (dbCommand.ExecuteNonQuery() == 1)
                     {
                         isDeleted = true;
                     }
                 }
             }
             db2Connection.Close();
             return isDeleted;
         }

          public static bool Update(MovieModel item)
           {
               bool isUpdated = false;

               MySqlConnection db2Connection = new MySqlConnection();
               db2Connection.Open();
               if (db2Connection.State == ConnectionState.Open)
               {
                   string dbQuery = string.Format("SELECT COUNT(Id) FROM Filmy WHERE Name = '{0}'", item.Nazwa);
                   MySqlCommand dbCommand = new MySqlCommand(dbQuery, db2Connection);
                   long result = (long)dbCommand.ExecuteScalar();
                   if (result == 1)
                   {
                       dbQuery = string.Format("UPADATE Filmy SET Nazwa = '{0}', Rok = '{1}', Autor = '{2}', PhotoName = '{3} WHERE Id = {4}", item.Nazwa, item.Rok, item.Autor, item.PhotoName);
                       dbCommand.CommandText = dbQuery;

                       dbCommand.ExecuteNonQuery();
                       isUpdated = true;
                   }   }
               db2Connection.Close();
               return isUpdated;
           }  }}
