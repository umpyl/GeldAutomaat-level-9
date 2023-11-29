using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Geldautomaat.classes
{
    internal class RekeningToevoegen : Model
    {
        string rekeningnmr;
        MainWindow mw = new MainWindow();
        public int GenerateRandomNumber(int min_value, int max_value)
        {
            Random random = new Random();
            return random.Next(min_value, max_value + 1);
        }

        public bool RekeningToevoege(int saldo, string encryted_pincode)
        {
            try
            {
                rekeningnmr = mw.EncryptString(GenerateRandomNumber(1000, 9999).ToString());

                string controlequery = "SELECT COUNT(*) FROM rekeningen WHERE rekeningnummer = @rekeningnummer";

                using (MySqlCommand cmd = new MySqlCommand(controlequery, db.connection))
                {
                    cmd.Parameters.AddWithValue("@rekeningnummer", rekeningnmr);

                    int controle = Convert.ToInt32(cmd.ExecuteScalar());

                    if(controle == 0)
                    {
                        string query = "INSERT INTO rekeningen(rekeningnummer, saldo, pincode) VALUES (@rekening_id, @saldo, @pincode)";

                        using (MySqlCommand command = new MySqlCommand(query, db.connection))
                        {
                            command.Parameters.AddWithValue("@rekening_id", rekeningnmr);
                            command.Parameters.AddWithValue("@saldo", saldo);
                            command.Parameters.AddWithValue("@pincode", encryted_pincode);

                            int count = command.ExecuteNonQuery();

                            if (count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

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
