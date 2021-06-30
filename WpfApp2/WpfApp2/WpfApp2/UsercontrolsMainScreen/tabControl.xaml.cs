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
        public string meterstand;
        public int typeId;
        public DateTime latestScan;
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

            overzichtInkt InktOverzicht = new overzichtInkt();

            InktOverzicht.plotterId = plotterId;
            InktOverzicht.loadData();

            ParentForm.MainGridOverzicht.Children.Add(InktOverzicht);
            Grid.SetColumn(InktOverzicht, 0);
            Grid.SetRow(InktOverzicht, 1);

            tellerStand tellerstand = new tellerStand();

            tellerstand.lblTellerstand.Content = meterstand;

            ParentForm.tellertstandGrid.Children.Add(tellerstand);
            Grid.SetColumn(tellerstand, 0);
            Grid.SetRow(tellerstand, 1);

            Controls controllers = new Controls();

            controllers.plotterId = plotterId;
            controllers.plotterNaam = lblTabName.Content.ToString();
            controllers.typeId = typeId;
            controllers.plotterIp = plotterIp;
            controllers.ParentForm = ParentForm;
            controllers.tabForm = this;
            controllers.lblLatestScanDate.Content = latestScan.ToString();
            controllers.meterstand = meterstand;
            controllers.serialnm = serialnm;

            ParentForm.statusGrid.Children.Add(controllers);
            Grid.SetColumn(controllers, 0);
            Grid.SetRow(controllers, 0);
            //Grid.SetColumnSpan(controllers, 2);

            controllers.SendPing();
            controllers.SetTimer();

            //SetTimer();
            //SendPing();
        }

        public void clear_screen()
        {
            //if (ParentForm.MainGridOverzicht.ColumnDefinitions.Count >= 1)
            //{
            //    ParentForm.MainGridOverzicht.ColumnDefinitions.RemoveRange(0, ParentForm.MainGridOverzicht.ColumnDefinitions.Count);
            //}

            ParentForm.MainGridOverzicht.Children.Clear();
            ParentForm.tellertstandGrid.Children.Clear();
            ParentForm.statusGrid.Children.Clear();
        }

        private void Grid_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ParentForm.blackenTabs();

            DotOne.Foreground = new SolidColorBrush(Colors.Red);
            DotTwo.Foreground = new SolidColorBrush(Colors.Red);
            lblTabName.Foreground = new SolidColorBrush(Colors.Red);

            clear_screen();
            loadData(); 
        }
    }
}
