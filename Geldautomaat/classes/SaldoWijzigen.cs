using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Geldautomaat.classes
{

    public class SaldoWijzigen : Model
    {
        //transactie_type 0 = storten
        //transactie_type 1 = pinnen
        public bool SaldoWijzig(string Encrypted_rekeningnmr, int saldo, int Pin_Bedrag, int medewerker)
        {
            try
            {
                if (Pin_Bedrag > saldo)
                {
                    return false;
                    
                }
                else
                {
                    string SelectQuery = "SELECT * FROM `transacties` WHERE `Rekeningen_rekeningnummer` = @rekeningnummer AND DATE(transcatie_tijd) = CURDATE() AND transactie_type = @type AND Werknemers_werknemer_id = @medewerker ORDER BY transcatie_tijd DESC LIMIT 3";

                    using (MySqlCommand SelectCmd = new MySqlCommand(SelectQuery, db.connection))
                    {
                        SelectCmd.Parameters.AddWithValue("@rekeningnummer", Encrypted_rekeningnmr);
                        SelectCmd.Parameters.AddWithValue("@type", 1);
                        SelectCmd.Parameters.AddWithValue("@medewerker", DBNull.Value);

                        int rowCount = 0;

                        using (MySqlDataReader reader = SelectCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rowCount++;
                            }
                        }

                        if (rowCount < 3)
                        {
                            int NieuweSaldo = saldo - Pin_Bedrag;
                            string query = "UPDATE rekeningen SET saldo = @saldo WHERE rekeningnummer = @rekeningnummer";

                            using (MySqlCommand cmd = new MySqlCommand(query, db.connection))
                            {
                                cmd.Parameters.AddWithValue("@rekeningnummer", Encrypted_rekeningnmr);
                                cmd.Parameters.AddWithValue("@saldo", NieuweSaldo);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    string insertQuery = "INSERT INTO transacties (bedrag, transactie_type, Rekeningen_rekeningnummer, Werknemers_werknemer_id) VALUES (@bedrag, @transactie_type,@rekeningen_rekeningnummer, @medewerker_id)";

                                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, db.connection))
                                    {
                                        insertCmd.Parameters.AddWithValue("@transactie_type", 1);
                                        insertCmd.Parameters.AddWithValue("@Rekeningen_rekeningnummer", Encrypted_rekeningnmr);
                                        if(medewerker > 0)
                                        {
                                            insertCmd.Parameters.AddWithValue("@bedrag", saldo);
                                            insertCmd.Parameters.AddWithValue("@medewerker_id", medewerker);
                                        }
                                        else
                                        {
                                            insertCmd.Parameters.AddWithValue("@bedrag", Pin_Bedrag);
                                            insertCmd.Parameters.AddWithValue("@medewerker_id", DBNull.Value);
                                        }
                                        

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
                        else
                        {
                            return false;
                        }
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
