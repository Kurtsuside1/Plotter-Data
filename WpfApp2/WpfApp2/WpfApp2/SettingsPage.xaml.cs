using System;
using System.Collections.Generic;
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
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using PlotterDataGH.Properties;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnFactorySettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Weet je het zeker?", "Fabrieksinstellingen", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SqliteConnection cnn1;
                cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                cnn1.Open();

                string query = string.Format("Delete from users");
                //create command and assign the query and connection from the constructor
                SqliteCommand cmd1 = new SqliteCommand(query, cnn1);

                //Execute command
                cmd1.ExecuteNonQuery();

                cnn1.Close();

                InitSetup ISP = new InitSetup();
                ISP.Show();
                this.Close();
            }
        }

        private void btnVerder_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnEditSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.editingAccount = true;
            InitSetup mw = new InitSetup();
            mw.editingMode();
            mw.Show();
            this.Close();
            Settings.Default.editingAccount = false;
            Settings.Default.Save();
        }
    }
}
