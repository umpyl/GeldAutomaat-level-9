using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Controls;

namespace Geldautomaat.classes
{
    internal class CheckTransactie : Model
    {
        public DataTable dataTabel { get; private set; } = new DataTable();

        public void GetTransacties(string rekeningnummer)
        {
            string selectQuery = "SELECT * FROM transacties WHERE Rekeningen_rekeningnummer = @rekeningnummer ORDER BY transcatie_tijd DESC LIMIT 3";

            using (MySqlCommand cmd = new MySqlCommand(selectQuery, db.connection))
            {
                cmd.Parameters.AddWithValue("rekeningnummer", rekeningnummer);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTabel);
                }
            }
        }


    }
}
