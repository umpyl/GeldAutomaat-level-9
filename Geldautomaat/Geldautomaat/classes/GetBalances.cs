using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geldautomaat.classes
{
    public class GetBalances : Model
    {
        public int GetBalance(string rekeningnummer)
        {
            try
            {
                string query = "SELECT saldo FROM rekeningen WHERE rekeningnummer = @rekeningnummer";

                using (MySqlCommand cmd = new MySqlCommand(query, db.connection))
                {
                    cmd.Parameters.AddWithValue("rekeningnummer", rekeningnummer);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result); // Return the saldo
                    }
                    else
                    {
                        return -1; // Credentials not found
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle eventuele fouten
                Console.WriteLine("Error: " + ex.Message);
                return -1; // Er is een fout opgetreden
            }
        }
    }
}
