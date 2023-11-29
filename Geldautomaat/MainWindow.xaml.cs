using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Geldautomaat.classes;
using System.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Geldautomaat
{
    
    public partial class MainWindow : Window
    {
        GetBalances GetBalances = new GetBalances();
        CheckCredentials credentialsChecker = new CheckCredentials();
        moneydeposits depositmoney = new moneydeposits();
        SaldoWijzigen SaldoWijzigen = new SaldoWijzigen();
        CheckTransactie checkTransactie = new CheckTransactie();


        private string schermnaam;
        string encrypted_rekeningnmr;
        string Encrypted_pincode;
        int saldo;
        int Stort_bedrag;
        int Pin_bedrag;

        //moet nog die message.box veranderen naar mooie labels
        private void Inlog_Check_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Inlog_Check_pincode.Text) && !string.IsNullOrWhiteSpace(Inlog_Check_pincode.Text))
            {
                Encrypted_pincode = EncryptString(Inlog_Check_pincode.Text);
                encrypted_rekeningnmr = EncryptString(Inlog_Check_Rekeningnmr.Text);

                bool InlogGelukt = credentialsChecker.CheckCredential(encrypted_rekeningnmr, Encrypted_pincode);
                saldo = GetBalances.GetBalance(encrypted_rekeningnmr);
                if (InlogGelukt == true) 
                {
                    Inlog_label.Visibility = Visibility.Visible;
                    Inlog_label.Content = "Inloggen Gelukt!";
                    scherm("Hoofdmenu");
                    Rekening_bedrag.Content = saldo;
                    Rekening_nummer.Content = Inlog_Check_Rekeningnmr.Text;
                }
                else
                {
                    Inlog_label.Visibility = Visibility.Visible;
                    Inlog_label.Content = "Inloggen Niet Gelukt!";
                    Inlog_Check_Rekeningnmr.Text = "";
                    Inlog_Check_pincode.Text = "";
                }
            }
            else
            {
                MessageBox.Show("niet ingevuld");
                Inlog_label.Visibility = Visibility.Visible;
                Inlog_label.Content = "Inloggen niet gelukt!";
                Inlog_Check_Rekeningnmr.Text = "";
                Inlog_Check_pincode.Text = "";
            }
        }

        private void Geld_storten_Click(object sender, RoutedEventArgs e)
        {
            scherm("Geld_storten");
        }

        private void Geld_storten_button_Click(object sender, RoutedEventArgs e)
        {
            Stort_bedrag = Convert.ToInt32(Geld_storten_bedrag.Text);

            if (Stort_bedrag > 0)
            {
                bool StortenGelukt = depositmoney.moneydeposit(encrypted_rekeningnmr, saldo, Stort_bedrag);

                if (StortenGelukt)
                {
                    MessageBox.Show("Deposit successful!");
                    scherm("Hoofdmenu");
                    saldo = GetBalances.GetBalance(encrypted_rekeningnmr);
                    Rekening_bedrag.Content = saldo;
                }
                else
                {
                    MessageBox.Show("Failed to deposit money.");
                    scherm("Hoofdmenu");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid positive amount to deposit.");
            }
        }

        private void Geld_opnemen_Click(object sender, RoutedEventArgs e)
        {
            scherm("Geld_opnemen");
        }

        private async void Geld_opnemen_button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(Geld_opnemen_bedrag.Text))
            {
                Pin_bedrag = Convert.ToInt32(Geld_opnemen_bedrag.Text);
                if(Pin_bedrag > 500) 
                {
                    Geld_opnemen_bedrag_text.Content = "Uw kunt max 500 pinnen!";
                    await Task.Delay(2500);
                    Geld_opnemen_bedrag_text.Content = "Hoeveel wilt u pinnen?";
                }
                else
                {
                    bool PinnenGelukt = SaldoWijzigen.SaldoWijzig(encrypted_rekeningnmr, saldo, Pin_bedrag,0);

                    if (PinnenGelukt)
                    {
                        Geld_opnemen_bedrag_text.Content = "Pinnen Gelukt!";
                        await Task.Delay(2500);
                        Geld_opnemen_bedrag.Text = "0";
                        scherm("Hoofdmenu");
                        saldo = GetBalances.GetBalance(encrypted_rekeningnmr);
                        Rekening_bedrag.Content = saldo;
                    }
                    else
                    {
                        Geld_opnemen_bedrag_text.Content = "Uw saldo is te laag!";
                        await Task.Delay(2500);
                        Geld_opnemen_bedrag_text.Content = "Hoeveel wilt u pinnen?";
                    }
                }
            }
            else
            {
                Geld_opnemen_bedrag_text.Content = "U heeft niks ingevuld!";
                await Task.Delay(2500);
                Geld_opnemen_bedrag_text.Content = "Hoeveel wilt u pinnen?";
            }
            
        }

        private void Laatste_transacties_Click(object sender, RoutedEventArgs e)
        {
            scherm("Transactie_menu");
            checkTransactie.GetTransacties(encrypted_rekeningnmr);
            dataGrid.ItemsSource = checkTransactie.dataTabel.DefaultView;
        }
        private void CijfersOnly(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Suppress non-numeric input
            }
        }

        public string EncryptString(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void Inlog_admin_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.Show();
            this.Close();
        }

        private void scherm(string schermnaam)
        {
            if (schermnaam == "Inlog")
            {
                Inlog.Visibility = Visibility.Visible;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Geld_opnemen_menu.Visibility = Visibility.Collapsed;
                Geld_Stort_menu.Visibility = Visibility.Collapsed;
                Transactie_menu.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "Hoofdmenu")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Visible;
                Geld_opnemen_menu.Visibility = Visibility.Collapsed;
                Geld_Stort_menu.Visibility = Visibility.Collapsed;
                Transactie_menu.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "Geld_opnemen")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Geld_opnemen_menu.Visibility = Visibility.Visible;
                Geld_Stort_menu.Visibility = Visibility.Collapsed;
                Transactie_menu.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "Geld_storten")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Geld_opnemen_menu.Visibility = Visibility.Collapsed;
                Geld_Stort_menu.Visibility = Visibility.Visible;
                Transactie_menu.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "Transactie_menu")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Geld_opnemen_menu.Visibility = Visibility.Collapsed;
                Geld_Stort_menu.Visibility = Visibility.Collapsed;
                Transactie_menu.Visibility = Visibility.Visible;
            }
        }

        
    }
}
