using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geldautomaat.classes
{
    internal class moneydeposits : Model
    {
        //transactie_type 0 = storten
        //transactie_type 1 = pinnen
        public bool moneydeposit(string rekeningnummer, int saldo, int Stort_bedrag)
        {
            try
            {
                int NiueweSaldo = saldo + Stort_bedrag;
                string query = "UPDATE rekeningen SET saldo = @saldo WHERE rekeningnummer = rekeningnummer";
                string insertQuery = "INSERT INTO transacties (bedrag, transactie_type, Rekeningen_rekeningnummer, Werknemers_werknemer_id) VALUES (@bedrag, @transactie_type,@rekeningen_rekeningnummer, @medewerker_id)";


                using (MySqlCommand cmd = new MySqlCommand(query, db.connection))
                {
                    cmd.Parameters.AddWithValue("saldo", NiueweSaldo);
                    cmd.Parameters.AddWithValue("rekening_id", rekeningnummer);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, db.connection))
                        {
                            insertCmd.Parameters.AddWithValue("bedrag", Stort_bedrag);
                            insertCmd.Parameters.AddWithValue("transactie_type", 0);
                            insertCmd.Parameters.AddWithValue("Rekeningen_rekeningnummer", rekeningnummer);
                            insertCmd.Parameters.AddWithValue("medewerker_id", DBNull.Value);
                            //hier moet nog komen als er een medewerker_id is dat je dan hier de id van de medewerker invult

                            int insertRowsAffected = insertCmd.ExecuteNonQuery();

                            return true;
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
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}
