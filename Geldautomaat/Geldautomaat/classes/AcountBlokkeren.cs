using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geldautomaat.classes
{
    class AcountBlokkeren : Model
    {
        public bool AccountBlok(string rekeningnummer, int accountstatus)
        {
            try
            {
                string updateQuery = "UPDATE rekeningen SET geblokkeerd = @blokkeren WHERE rekeningnummer = @rekeningnummer";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, db.connection))
                {
                    cmd.Parameters.AddWithValue("@rekeningnummer", rekeningnummer);
                    cmd.Parameters.AddWithValue("@blokkeren", 1);

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
