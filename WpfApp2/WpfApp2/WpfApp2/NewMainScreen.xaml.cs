﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using PlotterDataGH.Properties;
using System.IO;
using Microsoft.Data.Sqlite;
using SendFileTo;
using System.Reflection;
using Microsoft.Win32.TaskScheduler;
using WpfApp2;

namespace PlotterDataGH
{
    /// <summary>
    /// Interaction logic for NewMainScreen.xaml
    /// </summary>
    public partial class NewMainScreen : Window
    {
        DataTable dataTable = new DataTable();
        public NewMainScreen()
        {
            InitializeComponent();
            WindowState oldstate = WindowState;
            WindowState = WindowState.Maximized;
            Visibility = Visibility.Collapsed;
            //WindowStyle = WindowStyle.None;
            //ResizeMode = ResizeMode.NoResize;
            Visibility = Visibility.Visible;
            Activate();

            LoadData();
        }

        //Load the data from the local database file
        public void LoadData()
        {
            dataTable.Clear();
            SqliteConnection cnn;
            SqliteCommand cmd = null;
            cnn = new SqliteConnection("Data Source=plotterData.db;");
            cnn.Open();

            string query = "SELECT m1.*, models.plotter_type FROM printer_data m1 LEFT JOIN printer_data m2 ON (m1.serial_number = m2.serial_number AND m1.id < m2.id) INNER JOIN models on models.id = m1.model_id WHERE m2.id IS NULL";
            cmd = new SqliteCommand(query, cnn);

            SqliteDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = GridLength.Auto;
                tabControlGrid.RowDefinitions.Add(rd);

                tabControl tc = new tabControl();

                tc.lblTabName.Content = string.Format(row["naam"].ToString());
                tc.plotterId = Convert.ToInt32(row["id"]);
                tc.ParentForm = this;
                tc.meterstand = string.Format(row["meters_printed"].ToString());
                //tc.loadData();

                tabControlGrid.Children.Add(tc);
                Grid.SetRow(tc, tabControlGrid.RowDefinitions.Count - 1);
            }

            //Create a CSV file for mailing
            try
            {
                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));



                DataTable cartridgeTable = new DataTable();

                foreach (DataRow row in dataTable.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(",", fields));

                    SqliteConnection cnn1;
                    SqliteCommand cmd1 = null;
                    cnn1 = new SqliteConnection("Data Source=plotterData.db;");
                    cnn1.Open();

                    string query1 = string.Format("SELECT * FROM `cartridge_reading` where `parent_id` = {0}", row[0]);
                    cmd1 = new SqliteCommand(query1, cnn1);

                    SqliteDataReader reader1 = cmd1.ExecuteReader();

                    cartridgeTable.Load(reader1);
                }

                File.WriteAllText("plotterData.csv", sb.ToString());

                StringBuilder sb1 = new StringBuilder();

                IEnumerable<string> columnNames1 = cartridgeTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb1.AppendLine(string.Join(",", columnNames1));

                foreach (DataRow row1 in cartridgeTable.Rows)
                {
                    IEnumerable<string> fields = row1.ItemArray.Select(field => field.ToString());
                    sb1.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText("cartridgeData.csv", sb1.ToString());
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Please close Excel");
            }

        }

        private void btnAddPlotter_Click(object sender, RoutedEventArgs e)
        {
            AddPlotter addPlotter = new AddPlotter();
            addPlotter.ParentForm = this;
            addPlotter.Show();
        }


        #region Scanning
        public void RunScan(string Merk, string IP, string Naam)
        {
            var debugField = System.IO.Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().GetName().CodeBase);

            debugField = debugField.Substring(6);

            var filename = debugField + @"/ghWebscraper.exe";

            //Start the Converted python file and pass the paramater
            string arguments = string.Format(@"{0} {1} {2} {3}", Merk, IP, Settings.Default.bedrijfsNaam, Naam);

            //Process myProcess = new Process();
            //myProcess.Exited += new EventHandler(myProcess_Exited);
            //myProcess.StartInfo.FileName = filename;
            //myProcess.StartInfo.Arguments = arguments;
            //myProcess.Start();

            doStuff(filename, arguments);
        }

        async System.Threading.Tasks.Task doStuff(string fileName, string args)
        {
            //DisableWhileScanning();
            await RunProcessAsync(fileName, args);

            //MahApps.Metro.IconPacks.PackIconFontAwesome fe = btnScan.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
            //fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.BinocularsSolid;
            //btnScan.IsEnabled = true;

            LoadData();
        }

        public static async Task<int> RunProcessAsync(string fileName, string args)
        {
            using (var process = new Process
            {
                StartInfo =
        {
            FileName = fileName, Arguments = args,
            UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        },
                EnableRaisingEvents = true
            })
            {
                return await RunProcessAsync(process).ConfigureAwait(false);
            }
        }
        private static Task<int> RunProcessAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, ea) => Console.WriteLine(ea.Data);
            process.ErrorDataReceived += (s, ea) => Console.WriteLine("ERR: " + ea.Data);

            bool started = process.Start();
            if (!started)
            {
                //you may allow for the process to be re-used (started = false) 
                //but I'm not sure about the guarantees of the Exited event in such a case
                throw new InvalidOperationException("Could not start process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;

        }

        #endregion

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage sp = new SettingsPage();
            sp.Show();
            this.Close();
        }
    }
}
