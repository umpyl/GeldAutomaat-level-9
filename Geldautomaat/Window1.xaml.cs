using Geldautomaat.classes;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Geldautomaat
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MainWindow MW = new MainWindow();
        CheckAdmin check = new CheckAdmin();
        SaldoWijzigen saldowijzigen = new SaldoWijzigen();
        MainWindow mainwindow = new MainWindow();
        PincodeWijzigen pincodewijzigen = new PincodeWijzigen();
        AcountBlokkeren accountblokkeren = new AcountBlokkeren();
        RekeningToevoegen rekeningtoevoegen = new RekeningToevoegen();
        string Wachtwoord;
        string Naam;
        int medewerker;
        string schermnaam;
        string encrypted_rekeningnmr;
        int Saldo;
        bool controle;
        string NewPincode;
        int AccountStatus;
        bool buttonClikced;

        public Window1()
        {
            InitializeComponent();
        }

        //inlog checken
        public void admin_Check_Button_Click(object sender, RoutedEventArgs e)
        {
            Wachtwoord = MW.EncryptString(admin_Check_wachtwoord.Text);
            Naam = admin_Check_gebruikersnaam.Text;

            medewerker = check.Check(Naam, Wachtwoord);

            if (medewerker > 0)
            {
                scherms("Hoofdmenu");
            }
            else
            {
                MessageBox.Show("zebbie");
            }
        }

        //pincode wijzigen
        private async void Pincode_wijzigen_button_Click(object sender, RoutedEventArgs e)
        {
            NewPincode = mainwindow.EncryptString(pincode_wijzigen_NewPin.Text);
            encrypted_rekeningnmr = mainwindow.EncryptString(pincode_wijzigen_rekeningnmr.Text);
            controle = pincodewijzigen.PinWijzigen(encrypted_rekeningnmr, NewPincode);

            if (!string.IsNullOrWhiteSpace(pincode_wijzigen_rekeningnmr.Text) && !string.IsNullOrWhiteSpace(pincode_wijzigen_NewPin.Text))
            {
                if(controle == true)
                {
                    pincode_wijzigen_label.Content = "Pincode Gewijzigd";

                    await Task.Delay(2500);

                    pincode_wijzigen_label.Content = "Pincode Wijzigen";

                    scherms("Hoofdmenu");

                    pincode_wijzigen_NewPin.Text = "";
                    pincode_wijzigen_rekeningnmr.Text = "";
                }
                else
                {
                    pincode_wijzigen_label.Content = "Rekeningnummer bestaat niet ";

                    await Task.Delay(2500);

                    pincode_wijzigen_label.Content = "Pincode wijzigen";
                }
            }
            else
            {
                pincode_wijzigen_label.Content = "Rekeningnummer of nieuwe pincode is leeg";

                await Task.Delay(2500);

                pincode_wijzigen_label.Content = "Pincode wijzigen";
            }

        }


        //Voor saldo wijzigen van de klant
        private async void Saldo_wijzgen_button_Click(object sender, RoutedEventArgs e)
        {
            encrypted_rekeningnmr = mainwindow.EncryptString(Saldo_wijzgen_rekeningnmr.Text);
            Saldo = Convert.ToInt32(Saldo_wijzgen_NieuweSaldo.Text);
            controle = saldowijzigen.SaldoWijzig(encrypted_rekeningnmr, Saldo, 0, medewerker);

            if (!string.IsNullOrWhiteSpace(Saldo_wijzgen_rekeningnmr.Text) && !string.IsNullOrWhiteSpace(Saldo_wijzgen_NieuweSaldo.Text))
            {
                Saldo_wijzgen_title.Content = "Saldo gewijzigd";

                await Task.Delay(2500);

                Saldo_wijzgen_title.Content = "Saldo Wijzigen";

                scherms("Hoofdmenu");
            }
            else
            {
                Saldo_wijzgen_title.Content = "rekeningnummer of nieuwe saldo is leeg";

                await Task.Delay(2500);
                Saldo_wijzgen_title.Content = "Saldo Wijzigen";
            }
        }

        //Blokkeren of Deblokkeren
        private void rekening_deblokkeren_Click(object sender, RoutedEventArgs e)
        {
            AccountStatus = 0;
            buttonClikced = true;
        }

        private void rekening_blokkeren_Click(object sender, RoutedEventArgs e)
        {
            AccountStatus = 1;
            buttonClikced = true;
        }

        private async void rekening_blok_button_Click(object sender, RoutedEventArgs e)
        {
            encrypted_rekeningnmr = mainwindow.EncryptString(rekening_blok_text.Text);
            controle = accountblokkeren.AccountBlok(encrypted_rekeningnmr, AccountStatus);

            if (!string.IsNullOrWhiteSpace(rekening_blok_text.Text))
            {
                controle = accountblokkeren.AccountBlok(encrypted_rekeningnmr, AccountStatus);
                if(controle == true)
                {
                    rekening_blok_title.Content = "Rekening is " + RekeningBlokkeren() + " ";


                    await Task.Delay(2500);

                    rekening_blok_title.Content = "Rekening blokkeren of deblokkeren";
                    rekening_blok_text.Text = "";
                    buttonClikced = false;
                    scherms("Hoofdmenu");
                }
                else
                {
                    rekening_blok_title.Content = "Rekeningnummer bestaat niet ";


                    await Task.Delay(2500);

                    rekening_blok_title.Content = "Rekening blokkeren of deblokkeren";
                    rekening_blok_text.Text = "";
                }
            }
            else if(buttonClikced != true)
            {
                rekening_blok_title.Content = "Wilt u het Account blokkeren of deblokkeren?";

                await Task.Delay(2500);

                rekening_blok_title.Content = "Rekening blokkeren of deblokkeren";
            }
        }

        //zodat de text kon veranderen tussen geblokkeerd en blokkeren
        private string RekeningBlokkeren()
        {
            if(AccountStatus == 0)
            {
                return "Gedeblokkeerd";
            }
            else
            {
                return "Geblokkerd";
            }
        }


        // rekening toevoegen
        private async void rekening_toevoegen_button_Click(object sender, RoutedEventArgs e)
        {

            NewPincode = mainwindow.EncryptString(rekening_toevoegen_saldo.Text);

            controle = rekeningtoevoegen.RekeningToevoege(Convert.ToInt32(rekening_toevoegen_saldo.Text), NewPincode);

            if (!string.IsNullOrWhiteSpace(rekening_toevoegen_saldo.Text) && !string.IsNullOrWhiteSpace(rekening_toevoegen_pincode.Text))
            {
                if(controle == true)
                {
                    rekening_toevoegen_title.Content = "Rekening is toegevoegd!";


                    await Task.Delay(2500);

                    scherms("Hoofdmenu");
                    rekening_toevoegen_title.Content = "Rekening toevoegen";
                    rekening_toevoegen_saldo.Text = "";
                    rekening_toevoegen_pincode.Text = "";
                }
                else
                {
                    rekening_toevoegen_title.Content = "Rekening toevoegen is niet gelukt!";
                    await Task.Delay(2500);

                    rekening_toevoegen_title.Content = "Rekening toevoegen";
                    rekening_toevoegen_saldo.Text = "";
                    rekening_toevoegen_pincode.Text = "";
                }
                
            }
            else
            {
                rekening_toevoegen_title.Content = "Vul alle velden in!";


                await Task.Delay(2500);

                rekening_toevoegen_title.Content = "Rekening toevoegen";
                rekening_toevoegen_saldo.Text = "";
                rekening_toevoegen_pincode.Text = "";
            }
        }


        //De buttons om op de juiste pagina te komen
        private void Saldo_Wijzigen_Click(object sender, RoutedEventArgs e)
        {
            scherms("Saldo_wijzigen");
        }

        private void Pincode_wijzigen_Click(object sender, RoutedEventArgs e)
        {
            scherms("pincode_wijzigen");
        }

        private void Rekening_Blokkeren_button_Click(object sender, RoutedEventArgs e)
        {
            scherms("rekening_blokkeren");
        }

        private void Rekening_toevoegen_button_Click_1(object sender, RoutedEventArgs e)
        {
            scherms("rekening_toevoegen");
        }

        //functie om op het juiste pagina te komen
        private void scherms(string schermnaam)
        {
            if (schermnaam == "Inlog")
            {
                Inlog.Visibility = Visibility.Visible;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Saldo_Wijzgen.Visibility = Visibility.Collapsed;
                Rekening_blokkeren.Visibility = Visibility.Collapsed;
                Rekening_toevoegen.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "Hoofdmenu")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Visible;
                Saldo_Wijzgen.Visibility = Visibility.Collapsed;
                Rekening_blokkeren.Visibility = Visibility.Collapsed;
                Rekening_toevoegen.Visibility = Visibility.Collapsed;
                Pincode_wijzige.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "Saldo_wijzigen")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Saldo_Wijzgen.Visibility = Visibility.Visible;
                Rekening_blokkeren.Visibility = Visibility.Collapsed;
                Rekening_toevoegen.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "rekening_blokkeren")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Saldo_Wijzgen.Visibility = Visibility.Collapsed;
                Rekening_blokkeren.Visibility = Visibility.Visible;
                Rekening_toevoegen.Visibility = Visibility.Collapsed;
            }
            else if (schermnaam == "rekening_toevoegen")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Saldo_Wijzgen.Visibility = Visibility.Collapsed;
                Pincode_wijzigen.Visibility = Visibility.Collapsed;
                Rekening_blokkeren.Visibility = Visibility.Collapsed;
                Rekening_toevoegen.Visibility = Visibility.Visible;
            }
            else if (schermnaam == "pincode_wijzigen")
            {
                Inlog.Visibility = Visibility.Collapsed;
                Hoofdmenu.Visibility = Visibility.Collapsed;
                Saldo_Wijzgen.Visibility = Visibility.Collapsed;
                Pincode_wijzige.Visibility = Visibility.Visible;
                Rekening_blokkeren.Visibility = Visibility.Collapsed;
                Rekening_toevoegen.Visibility = Visibility.Collapsed;
            }
        }
    }
}
