using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using PlotterDataGH.Properties;
using PlotterDataGH;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;
using System.Reflection;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for InitSetup.xaml
    /// </summary>
    public partial class InitSetup : Window
    {
        bool editMode = false;
        bool sendData = true;
        private static readonly HttpClient client = new HttpClient();
        public InitSetup()
        {
            InitializeComponent();

            //If the account is not being edited create a new instance
            if (Settings.Default.editingAccount == false)
            {
                DataTable dataTable = new DataTable();
                SqliteConnection cnn;
                SqliteCommand cmd = null;
                cnn = new SqliteConnection("Data Source=plotterData.db;");
                cnn.Open();

                string query = "select bedrijfs_Naam from users";
                cmd = new SqliteCommand(query, cnn);

                SqliteDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);

                if (dataTable.Rows.Count == 1)
                {
                    MainWindow mw = new MainWindow();
                    Settings.Default.bedrijfsNaam = dataTable.Rows[0]["bedrijfs_Naam"].ToString();
                    Settings.Default.Save();
                    mw.Show();
                    MessageBox.Show("Hallo " + dataTable.Rows[0]["bedrijfs_Naam"]);
                    this.Close();
                }

                cbxSendData.IsChecked = false;
            }
            else
            {
                editMode = true;
            }

        }
                
        public void editingMode()
        {
            //If the account is being edited
            DataTable dataTable = new DataTable();
            SqliteConnection cnn;
            SqliteCommand cmd = null;
            cnn = new SqliteConnection("Data Source=plotterData.db;");
            cnn.Open();

            string query = "select * from users";
            cmd = new SqliteCommand(query, cnn);

            SqliteDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);

            tbxBedrijfsnaam.Text = dataTable.Rows[0]["bedrijfs_Naam"].ToString();
            tbxContactpersoon.Text = dataTable.Rows[0]["contactpersoon"].ToString();
            tbxEmail.Text = dataTable.Rows[0]["email"].ToString();
            tbxTelefoonnummer.Text = dataTable.Rows[0]["telefoonnummer"].ToString();
            cbxSendData.IsChecked = Settings.Default.sendData;
        }
            

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if(editMode == false)
            {
                if (tbxBedrijfsnaam.Text != "" && tbxContactpersoon.Text != "" && IsValidEmail(tbxEmail.Text) != false && tbxTelefoonnummer.Text != "")
                {

                    Settings.Default.sendData = false;
                    if (cbxSendData.IsChecked == true)
                    {
                        //Send account data to Goedhart Group
                        Await();
                    }

                    //Create a new account
                    SqliteConnection cnn1;
                    cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                    cnn1.Open();

                    string query = string.Format("INSERT INTO users (bedrijfs_Naam, contactpersoon, email, telefoonnummer) VALUES('{0}', '{1}', '{2}', '{3}')", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                    //create command and assign the query and connection from the constructor
                    SqliteCommand cmd1 = new SqliteCommand(query, cnn1);

                    //Execute command
                    cmd1.ExecuteNonQuery();

                    cnn1.Close();

                    Settings.Default.bedrijfsNaam = tbxBedrijfsnaam.Text;

                    Settings.Default.Save();

                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Info");
                }
            }
            else
            {
                if(IsValidEmail(tbxEmail.Text))
                {
                    Settings.Default.sendData = false;
                    if (cbxSendData.IsChecked == true)
                    {
                        //Send account data to Goedhart Group
                        Await();
                    }

                    //Update the current acount
                    SqliteConnection cnn1;
                    cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                    cnn1.Open();

                    string query = string.Format("UPDATE users SET bedrijfs_Naam = '{0}', contactpersoon = '{1}', email = '{2}', telefoonnummer = '{3}' WHERE id = 1 ", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                    //create command and assign the query and connection from the constructor
                    SqliteCommand cmd1 = new SqliteCommand(query, cnn1);

                    //Execute command
                    cmd1.ExecuteNonQuery();

                    cnn1.Close();

                    Settings.Default.bedrijfsNaam = tbxBedrijfsnaam.Text;

                    Settings.Default.Save();

                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("False Email");
                }
            }
            
        }

        async System.Threading.Tasks.Task Await()
        {
            //Send data to Goedhart Group
            int timeout = 1000;
            var task = SendUserAsync(tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
            if (await System.Threading.Tasks.Task.WhenAny(task, System.Threading.Tasks.Task.Delay(timeout)) == task)
            {
                // task completed within timeout
            }
            else
            {
                MessageBox.Show("Verbinding met de Goedhart Servers kon niet gemaakt worden");
            }
        }

        async System.Threading.Tasks.Task SendUserAsync(string bedrijfsnaam, string contactpersoon, string email, string telefoonnummer)
        {
            //Send data to Goedhart Group
            var values = new Dictionary<string, string>
            {
                { "postType", "Users" },
                { "bedrijfs_Naam", bedrijfsnaam },
                { "contactpersoon", contactpersoon },
                { "email", email },
                { "telefoonnummer", telefoonnummer }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://10.0.0.125/", content);

            var responseString = await response.Content.ReadAsStringAsync();

            MessageBox.Show(responseString);
        }

        bool IsValidEmail(string email)
        {
            //Check if the given email is valid
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void cbxSendData_Checked(object sender, RoutedEventArgs e)
        {
            //Messagebox for getting authorization to send data to Goedhart groep
            if (cbxSendData.IsChecked == false)
            {
                sendData = false;
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Goedhart Groep zal alleen de opgegeven profieldata en de data van de printers verkijgen", "Fabrieksinstellingen", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    sendData = true;
                }
                else
                {
                    cbxSendData.IsChecked = false;
                }
            }
        }
    }
}
