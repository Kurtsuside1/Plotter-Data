using MySql.Data.MySqlClient;
using PlotterDataGH.Properties;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PlotterDataGH;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Threading.Tasks;
using System;
using Microsoft.Win32.TaskScheduler;
using System.Net.Http;
using PlotterDataGH.UsercontrolsMainScreen;

namespace PlotterDataGH
{
    /// <summary>
    /// Interaction logic for tabControl.xaml
    /// </summary>
    public partial class tabControl : UserControl
    {
        bool cartridgeHidden = true;
        public int plotterId;
        public string plotterIp;
        public string serialnm;
        AutoResetEvent waiter = new AutoResetEvent(false);

        private static readonly HttpClient client = new HttpClient();

        public NewMainScreen ParentForm { get; set; }
        public tabControl()
        {
            InitializeComponent();
        }

        public void loadData()
        {
            //Load cartridge data and put it in a class called cartridgeControl
            DataTable dataTable = new DataTable();
            SqliteConnection cnn;
            SqliteCommand cmd = null;
            cnn = new SqliteConnection("Data Source=plotterData.db;");
            cnn.Open();

            string query = string.Format("SELECT * FROM `cartridge_reading` where `parent_id` = {0}", plotterId);
            cmd = new SqliteCommand(query, cnn);

            SqliteDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);

            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = GridLength.Auto;
            ParentForm.MainGrid.ColumnDefinitions.Add(cd);

            overzichtInkt InktOverzicht = new overzichtInkt();

            InktOverzicht.plotterId = plotterId;
            InktOverzicht.loadData();

            ParentForm.MainGrid.Children.Add(InktOverzicht);
            Grid.SetColumn(InktOverzicht, ParentForm.MainGrid.ColumnDefinitions.Count - 1);
            ColumnDefinition last = new ColumnDefinition();
            ParentForm.MainGrid.ColumnDefinitions.Add(last);

            //SetTimer();
            //SendPing();
        }

        private void Grid_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            loadData();
        }
    }
}
