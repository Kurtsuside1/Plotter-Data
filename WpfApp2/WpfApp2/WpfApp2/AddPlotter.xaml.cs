using MySql.Data.MySqlClient;
using PlotterDataGH.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PlotterDataGH;
using Microsoft.Data.Sqlite;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for AddPlotter.xaml
    /// </summary>
    public partial class AddPlotter : Window
    {
        public bool editingMode = false;
        int plotterId = 0;
        string serialNumber = "";

        public MainWindow ParentForm { get; set; }

        public AddPlotter()
        {
            InitializeComponent();

            DataTable dataTable = new DataTable();
            SqliteConnection cnn;
            SqliteCommand cmd = null;
            cnn = new SqliteConnection("Data Source=plotterData.db;");
            cnn.Open();

            string query = string.Format("SELECT * from models");
            cmd = new SqliteCommand(query, cnn);

            SqliteDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);


            foreach (DataRow dataRow in dataTable.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();

                item.Uid = dataRow["id"].ToString();
                item.Content = dataRow["plotter_type"].ToString();

                cbxPlotterType.Items.Add(item);
            }
        }

        public class plotter
        {
            public int ID { get; set; }
            public string serial_number { get; set; }
            public string plotterNaam { get; set; }
            public string plotterIP { get; set; }
            public string plotterType { get; set; }
            public string meters_printed { get; set; }
        }

        public void editForm(int plotterId_m)
        {
            plotterId = plotterId_m;
            DataTable dataTable = new DataTable();
            SqliteConnection cnn;
            SqliteCommand cmd = null;
            cnn = new SqliteConnection("Data Source=plotterData.db;");
            cnn.Open();

            string query = string.Format("SELECT printer_data.serial_number, printer_data.naam, printer_data.ip, models.plotter_type from `printer_data` INNER JOIN models ON printer_data.model_id = models.id where printer_data.id = {0} Limit 1", plotterId);
            cmd = new SqliteCommand(query, cnn);

            SqliteDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);

            serialNumber = dataTable.Rows[0]["serial_number"].ToString();
            tbxPlotIP.Text = dataTable.Rows[0]["IP"].ToString();
            tbxPlotNaam.Text = dataTable.Rows[0]["Naam"].ToString();
            cbxPlotterType.Text = dataTable.Rows[0]["plotter_type"].ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (editingMode)
            {
                SqliteConnection cnn;
                cnn = new SqliteConnection("Data Source=plotterData.db;");
                cnn.Open();

                string query = string.Format("UPDATE printer_data SET naam = '{0}', ip = '{1}' where id = {2}", tbxPlotNaam.Text, tbxPlotIP.Text, plotterId);
                //create command and assign the query and connection from the constructor
                SqliteCommand cmd = new SqliteCommand(query, cnn);

                //Execute command
                cmd.ExecuteNonQuery();

                cnn.Close();

                ParentForm.RunScan((cbxPlotterType.SelectedItem as ComboBoxItem).Uid.ToString(), tbxPlotIP.Text, tbxPlotNaam.Text);

                this.Close();
            }
            else if (tbxPlotIP.Text != "" && tbxPlotNaam.Text != "" && cbxPlotterType.Text != "")
            {
                //string connetionString;
                //MySqlConnection cnn;
                //connetionString = @"server=localhost;user id=root;password=;database=printer_data_test;";
                //cnn = new MySqlConnection(connetionString);
                //cnn.Open();


                ///CHANGE///
                //string query = string.Format("INSERT INTO printer_data(bedrijfs_Naam, contactpersoon, email, telefoonnummer) VALUES('{0}', '{1}', '{2}', '{3}')", tbxBedrijfsnaam.Text, tbxContactpersoon.Text, tbxEmail.Text, tbxTelefoonnummer.Text);
                //create command and assign the query and connection from the constructor
                // MySqlCommand cmd = new MySqlCommand(query, cnn);

                //Execute command
                //cmd.ExecuteNonQuery();

                //cnn.Close();
                //this.Close();

                //List<plotter> _plotter = new List<plotter>();
                //_plotter.Add(new plotter()
                //{
                //    ID = Settings.Default.plotterID,
                //    plotterNaam = tbxPlotNaam.Text,
                //    plotterIP = tbxPlotIP.Text,
                //    plotterType = tbxPlotType.Text,
                //    meters_printed = "0"
                //}) ;

                //string json = System.Text.Json.JsonSerializer.Serialize(_plotter);

                //string path = @"Plotter Data\Plotter.json";

                //if (Directory.Exists(path))
                //{
                //    File.WriteAllText(path, json);
                //}
                //else
                //{
                //    Directory.CreateDirectory("Plotter Data");
                //    File.WriteAllText(path, json);
                //}

                //Settings.Default.plotterID++;

                ParentForm.RunScan((cbxPlotterType.SelectedItem as ComboBoxItem).Uid.ToString(), tbxPlotIP.Text, tbxPlotNaam.Text);

                this.Close();
            }
            else
            {
                MessageBox.Show("Vul alstjeblieft alle velden in");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Weet je het zeker?", "Plotter Verwijderen", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SqliteConnection cnn1;
                cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                cnn1.Open();

                string query = string.Format("Delete from printer_data where serial_number = '{0}'", serialNumber);
                //create command and assign the query and connection from the constructor
                SqliteCommand cmd1 = new SqliteCommand(query, cnn1);

                //Execute command
                cmd1.ExecuteNonQuery();

                cnn1.Close();

                ParentForm.fillerGrid.RowDefinitions.Clear();
                ParentForm.fillerGrid.Children.Clear();
                ParentForm.LoadData();

                this.Close();

            }
        }
    }
}
