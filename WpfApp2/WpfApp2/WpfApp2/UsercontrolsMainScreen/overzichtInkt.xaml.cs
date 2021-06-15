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

namespace PlotterDataGH.UsercontrolsMainScreen
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class overzichtInkt : UserControl
    {
        public int plotterId = 0;

        public overzichtInkt()
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

            foreach (DataRow row in dataTable.Rows)
            {
                graphCollumn graphCollumns = new graphCollumn();
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = GridLength.Auto;
                graphGrid.ColumnDefinitions.Add(cd);

                graphCollumns.lblInkName.Content = row["cartridge_model"].ToString();
                graphCollumns.lblInkPerc.Content = row["volume"].ToString();

                string volume = row["volume"].ToString();
                volume = volume.Replace("%", "").ToString();

                graphCollumns.pgbPercentage.Value = Convert.ToInt32(volume);

                graphGrid.Children.Add(graphCollumns);
                Grid.SetColumn(graphCollumns, graphGrid.ColumnDefinitions.Count - 1);
            }
            ColumnDefinition last = new ColumnDefinition();
            graphGrid.ColumnDefinitions.Add(last);

            //for (int x = graphGrid.ColumnDefinitions.Count - 1; x > 0; x--)
            //{
            //    graphGrid.ColumnDefinitions[x].Width = new GridLength(0);
            //}

            //graphGrid.ColumnDefinitions[0].Width = GridLength.Auto;
        }
    }
}
