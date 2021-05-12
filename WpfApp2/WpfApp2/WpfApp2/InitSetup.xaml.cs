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
using System.Security.Cryptography;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for InitSetup.xaml
    /// </summary>
    public partial class InitSetup : Window
    {
        bool editMode = false;
        bool sendData = true;
        public InitSetup()
        {
            InitializeComponent();

            if (Settings.Default.editingAccount == false)
            {
                DataTable dataTable = new DataTable();
                SqliteConnection cnn;
                SqliteCommand cmd = null;
                cnn = new SqliteConnection("Data Source=plotterData.db;");
                cnn.Open();

                string query = "select contactpersoon from users";
                cmd = new SqliteCommand(query, cnn);

                SqliteDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);

                if (dataTable.Rows.Count == 1)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    MessageBox.Show("Hallo " + dataTable.Rows[0]["contactpersoon"]);
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

        public class data
        {
            public string Bedrijfsnaam { get; set; }
            public string Contactpersoon { get; set; }
            public string Email { get; set; }
            public string Telefoonnummer { get; set; }
            public bool SendData { get; set; }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if(editMode == false)
            {
                if (tbxBedrijfsnaam.Text != "" && tbxContactpersoon.Text != "" && tbxEmail.Text != "" && tbxTelefoonnummer.Text != "")
                {
                    using (Aes myAes = Aes.Create())
                    {

                        // Encrypt the string to an array of bytes.
                        byte[] encrypted = PublicMethods.EncryptStringToBytes_Aes(tbxContactpersoon.Text, myAes.Key, myAes.IV);

                        // Decrypt the bytes to a string.
                        string roundtrip = PublicMethods.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                        //Display the original data and the decrypted data.
                        Console.WriteLine("Original:   {0}", tbxContactpersoon.Text);
                        Console.WriteLine("Round Trip: {0}", roundtrip);
                    }




                    Settings.Default.sendData = false;
                    if (cbxSendData.IsChecked == true)
                    {
                        MySqlConnection cnn = mysql.openConnection();

                        string queryO = string.Format("INSERT INTO users (bedrijfs_Naam, contactpersoon, email, telefoonnummer) VALUES('{0}', '{1}', '{2}', '{3}')", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                        //create command and assign the query and connection from the constructor
                        MySqlCommand cmd = new MySqlCommand(queryO, cnn);

                        //Execute command
                        cmd.ExecuteNonQuery();
                        long userID = cmd.LastInsertedId;

                        Settings.Default.ID = Convert.ToInt32(userID);
                        Settings.Default.Save();

                        Settings.Default.sendData = true;

                        cnn.Close();
                    }

                    SqliteConnection cnn1;
                    cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                    cnn1.Open();

                    string query = string.Format("INSERT INTO users (bedrijfs_Naam, contactpersoon, email, telefoonnummer) VALUES('{0}', '{1}', '{2}', '{3}')", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                    //create command and assign the query and connection from the constructor
                    SqliteCommand cmd1 = new SqliteCommand(query, cnn1);

                    //Execute command
                    cmd1.ExecuteNonQuery();

                    cnn1.Close();


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
                Settings.Default.sendData = false;
                if (cbxSendData.IsChecked == true)
                {

                    MySqlConnection cnn = mysql.openConnection();
                    string queryO = string.Format("INSERT INTO users (bedrijfs_Naam, contactpersoon, email, telefoonnummer) VALUES('{0}', '{1}', '{2}', '{3}')", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(queryO, cnn);

                    //Execute command
                    cmd.ExecuteNonQuery();
                    long userID = cmd.LastInsertedId;

                    Settings.Default.ID = Convert.ToInt32(userID);
                    Settings.Default.Save();

                    Settings.Default.sendData = true;

                    cnn.Close();
                }

                SqliteConnection cnn1;
                cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                cnn1.Open();

                string query = string.Format("UPDATE users SET bedrijfs_Naam = '{0}', contactpersoon = '{1}', email = '{2}', telefoonnummer = '{3}' WHERE id = 1 ", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                //create command and assign the query and connection from the constructor
                SqliteCommand cmd1 = new SqliteCommand(query, cnn1);

                //Execute command
                cmd1.ExecuteNonQuery();

                cnn1.Close();

                Settings.Default.Save();

                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            
        }

        private void cbxSendData_Checked(object sender, RoutedEventArgs e)
        {
            if(cbxSendData.IsChecked == false)
            {
                sendData = false;
            }
            else
            {
                sendData = true;
            }
        }
    }
}
