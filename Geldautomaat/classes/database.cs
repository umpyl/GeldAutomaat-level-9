using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace Geldautomaat.classes
{
    public class myDBconnection
    {

        private static myDBconnection instance = null;

        public MySqlConnection connection;

        private myDBconnection()
        {
            // Define the connection string
            string connectionString = "Server=localhost;Uid=root;Pwd=;Database=geldautomaat";

            // Initialize the MySqlConnection object
            connection = new MySqlConnection(connectionString);
            Connect();
        }

        public static myDBconnection Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new myDBconnection();
                }
                return instance;
            }
        }

        // Method to open the database connection
        public bool Connect()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    return true; // Connection successful
                }
                else
                {
                    return false; // Connection is already open
                }
            }
            catch (Exception ex)
            {
                // Handle connection errors
                Console.WriteLine("Error: " + ex.Message);
                return false; // Connection failed
            }
        }

        // Method to close the database connection
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}

