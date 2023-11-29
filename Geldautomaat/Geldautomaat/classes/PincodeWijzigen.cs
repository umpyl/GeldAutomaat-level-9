using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geldautomaat.classes
{
    internal class PincodeWijzigen : Model
    {
        public bool PinWijzigen(string rekeningnummer, string NewPin)
        {
            try
            {
                string query = "UPDATE rekeningen SET pincode = @NewPin WHERE rekeningnummer = @rekeningnummer ";

                using (MySqlCommand cmd = new MySqlCommand(query, db.connection))
                {
                    cmd.Parameters.AddWithValue("@rekeningnummer", rekeningnummer);
                    cmd.Parameters.AddWithValue("@NewPin", NewPin);

                    int roweffected = cmd.ExecuteNonQuery();

                    if (roweffected > 0)
                    {
                        return true; 
                    }
                    else
                    {
                        return false; // Credentials not found
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle eventuele fouten
                Console.WriteLine("Error: " + ex.Message);
                return false; // Er is een fout opgetreden
            }
        }
    }
}
