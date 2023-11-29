using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geldautomaat.classes
{
    public class CheckCredentials : Model
    {
        public bool CheckCredential(string Encrypted_rekeningnmr, string Encrypted_pincode)
        {
            try
            {
                    string query = "SELECT COUNT(*) FROM rekeningen WHERE rekeningnummer = @rekeningnummer AND pincode = @pincode";

                    using (MySqlCommand cmd = new MySqlCommand(query, db.connection))
                    {
                        cmd.Parameters.AddWithValue("rekeningnummer", Encrypted_rekeningnmr);
                        cmd.Parameters.AddWithValue("pincode", Encrypted_pincode);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0 )
                        {
                            return true; 
                        }
                        else
                        {
                            return false; 
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
