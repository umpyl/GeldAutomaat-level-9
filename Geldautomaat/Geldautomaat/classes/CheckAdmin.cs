using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Geldautomaat.classes
{
    class CheckAdmin : Model
    {
        public int Check(string Naam, string Wachtwoord)
        {
            try
            {
                string query = "SELECT werknemer_id FROM werknemers WHERE naam = @naam AND wachtwoord = @wachtwoord";

                using (MySqlCommand cmd = new MySqlCommand(query, db.connection))
                {
                    cmd.Parameters.AddWithValue("@naam", Naam);
                    cmd.Parameters.AddWithValue("@wachtwoord", Wachtwoord);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result > 1)
                    {
                        return result; // Return the id
                    }
                }

                // If no valid id was found, return a default value
                return -1; // You can change this to an appropriate default value
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during database access
                Console.WriteLine("Error: " + ex.Message);

                // If an exception occurs, return a default value or throw another exception
                return -1; // Or throw a specific exception
            }
        }

    }
}
